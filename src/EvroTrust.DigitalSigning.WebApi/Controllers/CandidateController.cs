using EvroTrust.Infrastructure.Messaging;
using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EvroTrust.DigitalSigning.WebApi.Controllers
{
    [ApiController]
    // [Authorize]
    [Route("api/[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly IRabbitMqPublisherService _publisher;
        private readonly IOptions<RabbitMqOptions> _rabbitMqOptions;

        public CandidateController(IRabbitMqPublisherService publisher, IOptions<RabbitMqOptions> rabbitMqOptions)
        {
            _publisher = publisher;
            _rabbitMqOptions = rabbitMqOptions;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCandidate([FromBody] RegisterCandidateCommand command)
        {
            var candidateId = Guid.NewGuid();

            // Send message to RabbitMQ with all fields mapped
            await _publisher.InitializeAsync(_rabbitMqOptions.Value);
            await _publisher.PublishAsync("candidate.register", new RegisterCandidateCommand
            {
                FullName = command.FullName,
                Email = command.Email,
                Phone = command.Phone,
                DateOfBirth = command.DateOfBirth,
                Address = command.Address,
                City = command.City,
                Country = command.Country,
                LinkedInProfile = command.LinkedInProfile,
                ResumeUrl = command.ResumeUrl
            });

            return Ok(new { CandidateId = candidateId });
        }

        [HttpPost("assign-task")]
        public async Task<IActionResult> AssignTask([FromBody] AssignTaskCommand command)
        {
            await _publisher.InitializeAsync(_rabbitMqOptions.Value);
            await _publisher.PublishAsync("task.assign", new AssignTaskCommand
            {
                CandidateId = command.CandidateId,
                CodingTaskId = command.CodingTaskId,
                TaskTitle = command.TaskTitle,
                TaskDescription = command.TaskDescription,
                AssignedAt = command.AssignedAt,
                AssignedBy = command.AssignedBy,
                CandidateEmail = command.CandidateEmail,
                CandidateFullName = command.CandidateFullName
            });
            return Ok();
        }

        [HttpPost("upload-solution")]
        public async Task<IActionResult> UploadSolution([FromBody] UploadSolutionCommand command)
        {
            await _publisher.InitializeAsync(_rabbitMqOptions.Value);
            await _publisher.PublishAsync("solution.upload", new UploadSolutionCommand
            {
                CandidateId = command.CandidateId,
                CodingTaskId = command.CodingTaskId,
                CodeSolutionId = command.CodeSolutionId,
                EncryptedSolution = command.EncryptedSolution,
                UploadedAt = command.UploadedAt,
                FileName = command.FileName,
                FileType = command.FileType,
                CandidateEmail = command.CandidateEmail,
                CandidateFullName = command.CandidateFullName
            });
            return Ok();
        }

        [HttpGet("review-solution/{candidateId}")]
        public async Task<IActionResult> ReviewSolution(Guid candidateId)
        {
            // For demonstration, create a ReviewSolutionCommand with all fields mapped
            var reviewCommand = new ReviewSolutionCommand
            {
                CodeSolutionId = Guid.Empty, // Set appropriately in real use
                CandidateId = candidateId,
                CodingTaskId = Guid.Empty, // Set appropriately in real use
                Reviewer = string.Empty,
                IsReviewed = false,
                ReviewComments = null,
                Status = null,
                ReviewedAt = null,
                CandidateEmail = string.Empty,
                CandidateFullName = string.Empty
            };

            await _publisher.InitializeAsync(_rabbitMqOptions.Value);
            await _publisher.PublishAsync("solution.review", reviewCommand);

            // Return dummy solution for demonstration
            return Ok(new { CandidateId = candidateId, SolutionCode = "// candidate's code here" });
        }

        [HttpPost("final-decision")]
        public async Task<IActionResult> FinalDecision([FromBody] FinalDecisionCommand command)
        {
            await _publisher.InitializeAsync(_rabbitMqOptions.Value);
            await _publisher.PublishAsync("candidate.decision", new FinalDecisionCommand
            {
                DecisionId = command.DecisionId,
                CandidateId = command.CandidateId,
                CodeSolutionId = command.CodeSolutionId,
                Reviewer = command.Reviewer,
                Status = command.Status,
                Comments = command.Comments,
                DecidedAt = command.DecidedAt,
                CandidateEmail = command.CandidateEmail,
                CandidateFullName = command.CandidateFullName
            });
            return Ok();
        }
    }
}