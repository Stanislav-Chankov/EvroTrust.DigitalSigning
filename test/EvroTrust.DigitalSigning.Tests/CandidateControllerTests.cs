using System;
using System.Threading.Tasks;
using EvroTrust.DigitalSigning.WebApi.Controllers;
using EvroTrust.DigitalSigning.WebApi.Models.Responses;
using EvroTrust.Infrastructure.Messaging;
using EvroTrust.Infrastructure.Messaging.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace EvroTrust.DigitalSigning.Tests
{
    public class CandidateControllerTests
    {
        private readonly Mock<IRabbitMqPublisherService> _publisherMock;
        private readonly IOptions<RabbitMqOptions> _rabbitMqOptions;
        private readonly CandidateController _controller;

        public CandidateControllerTests()
        {
            _publisherMock = new Mock<IRabbitMqPublisherService>();
            _rabbitMqOptions = Options.Create(new RabbitMqOptions
            {
                HostName = "localhost",
                ExchangeName = "test-exchange",
                QueueName = "test-queue"
            });
            _controller = new CandidateController(_publisherMock.Object, _rabbitMqOptions);
        }

        [Fact]
        public async Task RegisterCandidate_ShouldPublishMessage_AndReturnCandidateId()
        {
            var command = new RegisterCandidateCommand
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Phone = "123456789",
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
                Address = "123 Main St",
                City = "Metropolis",
                Country = "Country",
                LinkedInProfile = "linkedin.com/johndoe",
                ResumeUrl = "resume.url"
            };

            _publisherMock.Setup(p => p.InitializeAsync(It.IsAny<RabbitMqOptions>())).Returns(Task.CompletedTask);
            _publisherMock.Setup(p => p.PublishAsync("candidate.register", It.IsAny<RegisterCandidateCommand>())).Returns(ValueTask.CompletedTask);

            var result = await _controller.RegisterCandidateAsync(command);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value as RegisterCandidateResponse;
            Assert.NotNull(value.CandidateId);

            _publisherMock.Verify(p => p.InitializeAsync(_rabbitMqOptions.Value), Times.Once);
            _publisherMock.Verify(p => p.PublishAsync("candidate.register", It.Is<RegisterCandidateCommand>(c =>
                c.FullName == command.FullName &&
                c.Email == command.Email &&
                c.Phone == command.Phone &&
                c.DateOfBirth == command.DateOfBirth &&
                c.Address == command.Address &&
                c.City == command.City &&
                c.Country == command.Country &&
                c.LinkedInProfile == command.LinkedInProfile &&
                c.ResumeUrl == command.ResumeUrl
            )), Times.Once);
        }

        [Fact]
        public async Task AssignTask_ShouldPublishMessage_AndReturnOk()
        {
            var command = new AssignTaskCommand
            {
                CandidateId = Guid.NewGuid(),
                CodingTaskId = Guid.NewGuid(),
                TaskTitle = "Task",
                TaskDescription = "Description",
                AssignedAt = DateTime.UtcNow,
                AssignedBy = "Admin",
                CandidateEmail = "candidate@example.com",
                CandidateFullName = "Candidate Name"
            };

            _publisherMock.Setup(p => p.InitializeAsync(It.IsAny<RabbitMqOptions>())).Returns(Task.CompletedTask);
            _publisherMock.Setup(p => p.PublishAsync("task.assign", It.IsAny<AssignTaskCommand>())).Returns(ValueTask.CompletedTask);

            var result = await _controller.AssignTaskAsync(command);

            Assert.IsType<OkResult>(result);

            _publisherMock.Verify(p => p.InitializeAsync(_rabbitMqOptions.Value), Times.Once);
            _publisherMock.Verify(p => p.PublishAsync("task.assign", It.Is<AssignTaskCommand>(c =>
                c.CandidateId == command.CandidateId &&
                c.CodingTaskId == command.CodingTaskId &&
                c.TaskTitle == command.TaskTitle &&
                c.TaskDescription == command.TaskDescription &&
                c.AssignedAt == command.AssignedAt &&
                c.AssignedBy == command.AssignedBy &&
                c.CandidateEmail == command.CandidateEmail &&
                c.CandidateFullName == command.CandidateFullName
            )), Times.Once);
        }

        [Fact]
        public async Task UploadSolution_ShouldPublishMessage_AndReturnOk()
        {
            var command = new UploadSolutionCommand
            {
                CandidateId = Guid.NewGuid(),
                CodingTaskId = Guid.NewGuid(),
                CodeSolutionId = Guid.NewGuid(),
                EncryptedSolution = new byte[] { 1, 2, 3 },
                UploadedAt = DateTime.UtcNow,
                FileName = "solution.zip",
                FileType = "zip",
                CandidateEmail = "candidate@example.com",
                CandidateFullName = "Candidate Name"
            };

            _publisherMock.Setup(p => p.InitializeAsync(It.IsAny<RabbitMqOptions>())).Returns(Task.CompletedTask);
            _publisherMock.Setup(p => p.PublishAsync("solution.upload", It.IsAny<UploadSolutionCommand>())).Returns(ValueTask.CompletedTask);

            var result = await _controller.UploadSolutionAsync(command);

            Assert.IsType<OkResult>(result);

            _publisherMock.Verify(p => p.InitializeAsync(_rabbitMqOptions.Value), Times.Once);
            _publisherMock.Verify(p => p.PublishAsync("solution.upload", It.Is<UploadSolutionCommand>(c =>
                c.CandidateId == command.CandidateId &&
                c.CodingTaskId == command.CodingTaskId &&
                c.CodeSolutionId == command.CodeSolutionId &&
                c.EncryptedSolution == command.EncryptedSolution &&
                c.UploadedAt == command.UploadedAt &&
                c.FileName == command.FileName &&
                c.FileType == command.FileType &&
                c.CandidateEmail == command.CandidateEmail &&
                c.CandidateFullName == command.CandidateFullName
            )), Times.Once);
        }

        [Fact]
        public async Task ReviewSolution_ShouldPublishMessage_AndReturnDummySolution()
        {
            var candidateId = Guid.NewGuid();

            _publisherMock.Setup(p => p.InitializeAsync(It.IsAny<RabbitMqOptions>())).Returns(Task.CompletedTask);
            _publisherMock.Setup(p => p.PublishAsync("solution.review", It.IsAny<ReviewSolutionCommand>())).Returns(ValueTask.CompletedTask);

            var result = await _controller.ReviewSolutionAsync(candidateId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value as ReviewSolutionResponse;
            Assert.Equal(candidateId, value.CandidateId);
            Assert.Equal("// candidate's code here", value.SolutionCode);

            _publisherMock.Verify(p => p.InitializeAsync(_rabbitMqOptions.Value), Times.Once);
            _publisherMock.Verify(p => p.PublishAsync("solution.review", It.Is<ReviewSolutionCommand>(c =>
                c.CandidateId == candidateId
            )), Times.Once);
        }

        [Fact]
        public async Task FinalDecision_ShouldPublishMessage_AndReturnOk()
        {
            var command = new FinalDecisionCommand
            {
                DecisionId = Guid.NewGuid(),
                CandidateId = Guid.NewGuid(),
                CodeSolutionId = Guid.NewGuid(),
                Reviewer = "Reviewer",
                Status = "Accepted",
                Comments = "Well done",
                DecidedAt = DateTime.UtcNow,
                CandidateEmail = "candidate@example.com",
                CandidateFullName = "Candidate Name"
            };

            _publisherMock.Setup(p => p.InitializeAsync(It.IsAny<RabbitMqOptions>())).Returns(Task.CompletedTask);
            _publisherMock.Setup(p => p.PublishAsync("candidate.decision", It.IsAny<FinalDecisionCommand>())).Returns(ValueTask.CompletedTask);

            var result = await _controller.AddFinalDecisionAsync(command);

            Assert.IsType<OkResult>(result);

            _publisherMock.Verify(p => p.InitializeAsync(_rabbitMqOptions.Value), Times.Once);
            _publisherMock.Verify(p => p.PublishAsync("candidate.decision", It.Is<FinalDecisionCommand>(c =>
                c.DecisionId == command.DecisionId &&
                c.CandidateId == command.CandidateId &&
                c.CodeSolutionId == command.CodeSolutionId &&
                c.Reviewer == command.Reviewer &&
                c.Status == command.Status &&
                c.Comments == command.Comments &&
                c.DecidedAt == command.DecidedAt &&
                c.CandidateEmail == command.CandidateEmail &&
                c.CandidateFullName == command.CandidateFullName
            )), Times.Once);
        }

        [Fact]
        public async Task RegisterCandidate_ShouldHandlePublisherException()
        {
            var command = new RegisterCandidateCommand
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Phone = "123456789",
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
                Address = "123 Main St",
                City = "Metropolis",
                Country = "Country",
                LinkedInProfile = "linkedin.com/johndoe",
                ResumeUrl = "resume.url"
            };

            _publisherMock.Setup(p => p.InitializeAsync(It.IsAny<RabbitMqOptions>())).ThrowsAsync(new ArgumentNullException("RabbitMqOptions cannot be null."));

            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.RegisterCandidateAsync(command));
        }

        [Fact]
        public async Task AssignTask_ShouldHandlePublisherException()
        {
            var command = new AssignTaskCommand
            {
                CandidateId = Guid.NewGuid(),
                CodingTaskId = Guid.NewGuid(),
                TaskTitle = "Task",
                TaskDescription = "Description",
                AssignedAt = DateTime.UtcNow,
                AssignedBy = "Admin",
                CandidateEmail = "candidate@example.com",
                CandidateFullName = "Candidate Name"
            };

            _publisherMock.Setup(p => p.InitializeAsync(It.IsAny<RabbitMqOptions>())).ThrowsAsync(new ArgumentNullException("RabbitMqOptions cannot be null."));

            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.AssignTaskAsync(command));
        }

        [Fact]
        public async Task UploadSolution_ShouldHandlePublisherException()
        {
            var command = new UploadSolutionCommand
            {
                CandidateId = Guid.NewGuid(),
                CodingTaskId = Guid.NewGuid(),
                CodeSolutionId = Guid.NewGuid(),
                EncryptedSolution = new byte[] { 1, 2, 3 },
                UploadedAt = DateTime.UtcNow,
                FileName = "solution.zip",
                FileType = "zip",
                CandidateEmail = "candidate@example.com",
                CandidateFullName = "Candidate Name"
            };

            _publisherMock.Setup(p => p.InitializeAsync(It.IsAny<RabbitMqOptions>())).ThrowsAsync(new ArgumentNullException("RabbitMqOptions cannot be null."));

            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.UploadSolutionAsync(command));
        }

        [Fact]
        public async Task ReviewSolution_ShouldHandlePublisherException()
        {
            var candidateId = Guid.NewGuid();

            _publisherMock.Setup(p => p.InitializeAsync(It.IsAny<RabbitMqOptions>())).ThrowsAsync(new ArgumentNullException("RabbitMqOptions cannot be null."));

            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.ReviewSolutionAsync(candidateId));
        }

        [Fact]
        public async Task FinalDecision_ShouldHandlePublisherException()
        {
            var command = new FinalDecisionCommand
            {
                DecisionId = Guid.NewGuid(),
                CandidateId = Guid.NewGuid(),
                CodeSolutionId = Guid.NewGuid(),
                Reviewer = "Reviewer",
                Status = "Accepted",
                Comments = "Well done",
                DecidedAt = DateTime.UtcNow,
                CandidateEmail = "candidate@example.com",
                CandidateFullName = "Candidate Name"
            };

            _publisherMock.Setup(p => p.InitializeAsync(It.IsAny<RabbitMqOptions>())).ThrowsAsync(new ArgumentNullException("RabbitMqOptions cannot be null."));

            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.AddFinalDecisionAsync(command));
        }
    }
}