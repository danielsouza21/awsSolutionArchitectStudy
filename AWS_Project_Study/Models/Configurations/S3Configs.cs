using System.Text.Json.Serialization;

namespace AWS_Project_Study.Models.Configurations
{
    public class S3Configs
    {

        public string AwsAccessKeyId { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string ServiceURL { get; set; }
    }
}
