using ExceptionFactories;
using OmniAppium.ConfigUtilityService.Services;
using TransversalUtilityServices;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public class ConfigValidator<T>
         where T : class
    {
        ITransversalService _transversalService;
        public ConfigValidator(ITransversalService transversalService)
        {
            this._transversalService = transversalService;
        }

        public void EnsureConfigs(T data)
        {
            try
            {
                this._transversalService.Transverse(data , (obj) => {
                    if(obj is IConfigValidator v)
                    {
                        v.EnsureConfig();
                    }
                });
            }
            catch(Exception ex)
            {
                // TODO:
                var message = new ExceptionFactory(ex).Create();
                throw;
            }
        }
        public void EnsureValid(T data)
        {
            try{
                this._transversalService.Transverse(data, (obj) => {
                    if (obj is IValidatable v) {
                        var result = v.Validate();
                        if(!result.IsValid)
                        {
                            throw new Exception($"Validation failed, {result.ErrorMessage}");
                        }
                    }
                });
            }
            catch (Exception ex) {
                // TODO:
                var message=new ExceptionFactory(ex).Create();
                throw;
            }
        }
    }
}
