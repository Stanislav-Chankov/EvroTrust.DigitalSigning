namespace EvroTrust.DigitalSigning.WebApi.Models.Responses
{
    public class ReviewSolutionResponse
    {
        public Guid CandidateId { get; set; }

        /// <summary>
        /// Gets or sets the solution code.
        /// </summary>
        /// <value>
        /// The solution code.
        /// </value>
        /// TODO: Store as base64 string or encrypted string in real use. Persist in database as needed.
        public string SolutionCode { get; set; }
    }
}
