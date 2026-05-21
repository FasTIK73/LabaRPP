using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RPP.Common.Exceptions;
using RPP.Common.Extensions;
using RPP.Common.Infrastructure;
using RPP.Common.Infrastructure.PostConfigurations;

namespace RPP.DataModels;

public class PostDataModel : IValidation
{
    public string Id { get; private set; }
    public string PostName { get; private set; }
    public int PostType { get; private set; }
    public PostConfiguration ConfigurationModel { get; private set; }

    public PostDataModel(string id, string postName, int postType, PostConfiguration configuration)
    {
        Id = id;
        PostName = postName;
        PostType = postType;
        ConfigurationModel = configuration;
    }

    public PostDataModel(string id, string postName, int postType, string configurationJson)
        : this(id, postName, postType, (PostConfiguration)null)
    {
        if (!string.IsNullOrEmpty(configurationJson))
        {
            var obj = JToken.Parse(configurationJson);
            ConfigurationModel = obj.Value<string>("Type") switch
            {
                nameof(CashierPostConfiguration) => JsonConvert.DeserializeObject<CashierPostConfiguration>(configurationJson)!,
                nameof(SupervisorPostConfiguration) => JsonConvert.DeserializeObject<SupervisorPostConfiguration>(configurationJson)!,
                _ => JsonConvert.DeserializeObject<PostConfiguration>(configurationJson)!
            };
        }
    }

    public void Validate()
    {
        if (Id.IsEmpty())
            throw new ValidationException("Field Id is empty");
        if (!Id.IsGuid())
            throw new ValidationException("The value in the field Id is not a unique identifier");
        if (PostName.IsEmpty())
            throw new ValidationException("Field PostName is empty");
        if (PostType == 0)
            throw new ValidationException("Field PostType is empty");
        if (ConfigurationModel is null)
            throw new ValidationException("Field ConfigurationModel is not initialized");
        if (ConfigurationModel.Rate <= 0)
            throw new ValidationException("Field Rate is less or equal zero");
    }
}