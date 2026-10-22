using System.IO.Abstractions;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using OCRUtilityServices.Services;
using OmniAppium.EngineUtilityServices.Services.Grounding;

namespace OmniAppiumDemo.DependencyInjection;

/// <summary>
/// Loads independently approved Issue 54 smoke inputs without creating approvals or claims.
/// </summary>
public static class Issue54SmokeProvisioningExtensions
{
    /// <summary>
    /// Adds protected-click dependencies only when the explicitly selected scenario is protected-valid.
    /// </summary>
    public static IServiceCollection AddIssue54SmokeProvisioning(
        this IServiceCollection services,
        string[] args)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(args);

        int scenarioIndex = Array.IndexOf(args, "--issue50-smoke");
        if (scenarioIndex < 0 || scenarioIndex + 1 >= args.Length ||
            !string.Equals(args[scenarioIndex + 1], "protected-valid", StringComparison.Ordinal))
            return services;

        if (!args.Contains("--issue54-protected-confirmed", StringComparer.Ordinal))
            return services;

        string? publicKeyPath = Environment.GetEnvironmentVariable("ISSUE54_TRUSTED_PUBLIC_KEY_PEM");
        string? reviewerId = Environment.GetEnvironmentVariable("ISSUE54_REVIEWER_ID");
        string? keyId = Environment.GetEnvironmentVariable("ISSUE54_KEY_ID");
        string? fingerprint = Environment.GetEnvironmentVariable("ISSUE54_PUBLIC_KEY_SPKI_SHA256");
        string? claimDirectory = Environment.GetEnvironmentVariable("ISSUE54_CLAIM_DIRECTORY");
        string? requestPath = Environment.GetEnvironmentVariable("ISSUE54_REQUEST_JSON");

        if (new[] { publicKeyPath, reviewerId, keyId, fingerprint, claimDirectory, requestPath }
            .Any(string.IsNullOrWhiteSpace))
            return services; // Smoke runner reports NOT RUN; never dispatch.

        IFileSystem fs = new FileSystem();
        if (!fs.File.Exists(publicKeyPath!) ||
            !fs.File.Exists(requestPath!) ||
            !fs.Directory.Exists(claimDirectory!))
            return services;

        string publicPem = fs.File.ReadAllText(publicKeyPath!);
        using ECDsa key = ECDsa.Create();
        key.ImportFromPem(publicPem);
        if (key.KeySize != 256 ||
            key.ExportParameters(false).Curve.Oid.Value != "1.2.840.10045.3.1.7")
            throw new InvalidOperationException("Issue 54 trusted key must be ECDSA P-256.");

        byte[] spki = key.ExportSubjectPublicKeyInfo();
        try
        {
            byte[] actual = SHA256.HashData(spki);
            byte[] expected;
            try
            {
                expected = Convert.FromHexString(fingerprint!);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("Issue 54 public key fingerprint is invalid.");
            }

            try
            {
                if (expected.Length != 32 ||
                    !CryptographicOperations.FixedTimeEquals(actual, expected))
                    throw new InvalidOperationException("Issue 54 public key fingerprint mismatch.");
            }
            finally
            {
                CryptographicOperations.ZeroMemory(expected);
                CryptographicOperations.ZeroMemory(actual);
            }
        }
        finally
        {
            CryptographicOperations.ZeroMemory(spki);
        }

        TrustedGroundedClickRequest request;
        try
        {
            byte[] bytes = fs.File.ReadAllBytes(requestPath!);
            try
            {
                request = JsonSerializer.Deserialize<TrustedGroundedClickRequest>(bytes)
                    ?? throw new InvalidOperationException("Issue 54 request is empty.");
            }
            finally
            {
                CryptographicOperations.ZeroMemory(bytes);
            }
        }
        catch (JsonException)
        {
            throw new InvalidOperationException("Issue 54 request JSON is invalid.");
        }

        if (request.Approval is null || request.Snapshot is null ||
            string.IsNullOrWhiteSpace(request.TargetId))
            throw new InvalidOperationException("Issue 54 request is incomplete.");

        var trustedKey = new TrustedReviewerKey(reviewerId!, keyId!, publicPem);
        services.AddSingleton(new Issue54ClaimDirectory(fs.Path.GetFullPath(claimDirectory!)));
        services.AddTrustedGroundedClick(new[] { trustedKey });
        services.AddSingleton(request);
        return services;
    }
}
