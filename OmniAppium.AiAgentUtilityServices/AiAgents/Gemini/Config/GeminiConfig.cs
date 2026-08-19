using AiUtility.GeminiUtilityServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.AiAgentUtilityServices.AiAgents.Gemini.Config
{
    public class GeminiConfig
    {
        private const int MAX_OUTPUT_TOKENS = 4096; 
        public GeminiGenerateRequest DefaultRequestConfig = new GeminiGenerateRequest {
            Prompt = string.Empty,
            Contents = new(),
            ResponseMimeType = "application/json" ,
            Temperature = 0.5,
            MaxOutputTokens = MAX_OUTPUT_TOKENS , ResponseSchema = new(),
            SafetySettings = new(), Tools =new()
        };
    }
}
