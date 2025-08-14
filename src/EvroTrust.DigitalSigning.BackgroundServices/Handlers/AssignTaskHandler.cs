using EvroTrust.DigitalSigning.BackgroundServices.Abstract;
using EvroTrust.DigitalSigning.Persistence.Abstract;
using EvroTrust.DigitalSigning.Persistence.Entities;
using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.EntityFrameworkCore;

namespace EvroTrust.DigitalSigning.BackgroundServices.Handlers
{
    public class AssignTaskHandler : IMessageHandler<AssignTaskCommand>
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AssignTaskHandler> _logger;
        private readonly IDigitalSigningDbContext _dbContext;
        private IEnumerable<string> contests = new[]
        {
            "https://codeforces.com/contest/2131/problem/A",
            "https://codeforces.com/contest/2131/problem/B",
            "https://codeforces.com/contest/2131/problem/C",
            "https://codeforces.com/contest/2131/problem/D",
            "https://codeforces.com/contest/2131/problem/E",
            "https://codeforces.com/contest/2131/problem/F",
            "https://codeforces.com/contest/2131/problem/G",
            "https://codeforces.com/contest/2131/problem/H",
        };

        public AssignTaskHandler(
            IEmailSender emailSender,
            ILogger<AssignTaskHandler> logger,
            IDigitalSigningDbContext dbContext)
        {
            _emailSender = emailSender;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task HandleAsync(AssignTaskCommand command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handled AssignTask for CandidateId: {CandidateId}, TaskTitle: {TaskTitle}", command.CandidateId, command.TaskTitle);

            var candidate = await _dbContext.Candidates.FirstOrDefaultAsync(c => c.CandidateId == command.CandidateId, cancellationToken)
                ?? throw new InvalidOperationException("Candidate not found.");

            var random = new Random();
            // Randomly select a contest from the predefined list
            var randomContest = contests.ElementAt(random.Next(contests.Count()));

            var codingTask = new CodingTask
            {
                CodingTaskId = command.CodingTaskId,
                Title = command.TaskTitle,
                Description = command.TaskDescription,
                AssignedAt = command.AssignedAt,
                AssignedBy = command.AssignedBy,
                ContestUrl = randomContest,
                CandidateId = command.CandidateId
            };

            _dbContext.CodingTasks.Add(codingTask);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Send email notification to the candidate
            var emailSubject = $"New Coding Task Assigned: {command.TaskTitle}";
            var emailBody = $@"
                <h1>New Coding Task Assigned</h1>
                <p>Dear {candidate.FullName},</p>
                <p>You have been assigned a new coding task:</p>
                <h2>{command.TaskTitle}</h2>
                <p>{command.TaskDescription}</p>
                <p>Contest URL: <a href='{randomContest}'>{randomContest}</a></p>
                <p>Assigned At: {command.AssignedAt}</p>
                <p>Assigned By: {command.AssignedBy}</p>
                <p>Good luck!</p>";
            try
                {
                await _emailSender.SendEmailAsync(candidate.Email, emailSubject, emailBody);
                _logger.LogInformation("Email sent to {Email} for assigned task {TaskTitle}", candidate.Email, command.TaskTitle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email} for assigned task {TaskTitle}", candidate.Email, command.TaskTitle);
                // TODO: Retry logic or fallback mechanism can be implemented here
                throw new InvalidOperationException("Failed to send email notification.", ex);
            }
        }
    }
}