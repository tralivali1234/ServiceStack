// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// HelloAllTypesResponse
    /// </summary>
    /// <remarks>
    /// HelloAllTypesResponse
    /// </remarks>
    public partial class HelloAllTypesResponse
    {
        /// <summary>
        /// Initializes a new instance of the HelloAllTypesResponse class.
        /// </summary>
        public HelloAllTypesResponse()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the HelloAllTypesResponse class.
        /// </summary>
        public HelloAllTypesResponse(string result = default(string), AllTypes allTypes = default(AllTypes), AllCollectionTypes allCollectionTypes = default(AllCollectionTypes))
        {
            Result = result;
            AllTypes = allTypes;
            AllCollectionTypes = allCollectionTypes;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Result")]
        public string Result { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AllTypes")]
        public AllTypes AllTypes { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AllCollectionTypes")]
        public AllCollectionTypes AllCollectionTypes { get; set; }

    }
}
