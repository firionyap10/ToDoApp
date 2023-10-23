using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApp.API.Controllers;
using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.Enums;
using TodoApp.API.Entities.Todos;
using TodoApp.API.Entities.ToDos.DTOs;
using TodoApp.API.Services.ToDos;

namespace TodoApp.Tests.Controllers
{
    public class ToDoControllerTests
    {
        private ToDoController _controller;
        private Mock<IToDoService> _mockToDoService;

        [SetUp]
        public void SetUp()
        {
            _mockToDoService = new Mock<IToDoService>();
            _controller = new ToDoController(_mockToDoService.Object);
        }

        [Test]
        public async Task Get_ReturnsToDoItem_WhenServiceReturnsItem()
        {
            var expectedToDo = new GetToDoResponse
            {
                Id = 1,
                Name = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Parse("2023-10-20T01:23:02.025"),
                Status = ToDoStatus.NotStarted,
                Priority = ToDoPriority.Medium,
                Tags = new List<Tag>(),
                CreatedDate = DateTime.Parse("2023-10-21T08:30:14.4777077"),
                UpdatedDate = null
            };

            _mockToDoService.Setup(service => service.GetAsync(It.IsAny<int>()))
                            .ReturnsAsync(expectedToDo);

            var result = await _controller.Get(1);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<GetToDoResponse>(okResult.Value);
            Assert.AreEqual(expectedToDo, okResult.Value);
        }

        [Test]
        public async Task Get_ReturnsNotFound_WhenServiceReturnsNull()
        {
            _mockToDoService.Setup(service => service.GetAsync(It.IsAny<int>()))
                            .ReturnsAsync((GetToDoResponse)null);

            var result = await _controller.Get(1);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task List_ReturnsPagedList_WhenServiceReturnsItems()
        {
            var expectedToDos = new PagedList<ListToDoResponse>(new List<ListToDoResponse>
            {
                new ListToDoResponse 
                {
                    Id = 1,
                    Name = "Task 1",
                    Description = "Description 1",
                    DueDate = DateTime.Parse("2023-10-20T01:23:02.025"),
                    Status = ToDoStatus.NotStarted,
                    Priority =  ToDoPriority.Medium,
                    Tags = new List<Tag>(),
                    CreatedDate = DateTime.Parse("2023-10-21T08:30:14.4777077"),
                    UpdatedDate = null 
                },
                new ListToDoResponse
                {
                    Id = 2,
                    Name = "Task 2",
                    Description = "Description 2",
                    DueDate = DateTime.Parse("2023-10-20T01:23:02.025"),
                    Status = ToDoStatus.NotStarted,
                    Priority =  ToDoPriority.Medium,
                    Tags = new List<Tag>(),
                    CreatedDate = DateTime.Parse("2023-10-21T08:30:14.4777077"),
                    UpdatedDate = null
                }
            }, 1, 1, 10);
            _mockToDoService.Setup(service => service.ListAsync(It.IsAny<ListToDoRequest>()))
                            .ReturnsAsync(expectedToDos);

            var result = await _controller.List(new ListToDoRequest());
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<PagedList<ListToDoResponse>>(okResult.Value);
            Assert.AreEqual(expectedToDos, okResult.Value);
        }

        [Test]
        public async Task List_ReturnsNotFound_WhenServiceReturnsNull()
        {
            _mockToDoService.Setup(service => service.ListAsync(It.IsAny<ListToDoRequest>()))
                            .ReturnsAsync((PagedList<ListToDoResponse>)null);

            var result = await _controller.List(new ListToDoRequest());
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Create_ReturnsToDoItem_WhenServiceCreatesItem()
        {
            var expectedToDo = new CreateToDoResponse 
            {
                Id = 2,
                Name = "Task 2",
                Description = "Description 2",
                DueDate = DateTime.Parse("2023-10-20T01:23:02.025"),
                Status = ToDoStatus.NotStarted,
                Priority = ToDoPriority.Medium,
                Tags = new List<Tag>(),
                CreatedDate = DateTime.Parse("2023-10-21T08:30:14.4777077")
            };
            _mockToDoService.Setup(service => service.CreateAsync(It.IsAny<CreateToDoRequest>()))
                            .ReturnsAsync(expectedToDo);

            var result = await _controller.Create(new CreateToDoRequest());
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<CreateToDoResponse>(okResult.Value);
            Assert.AreEqual(expectedToDo, okResult.Value);
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenServiceThrowsException()
        {
            _mockToDoService.Setup(service => service.CreateAsync(It.IsAny<CreateToDoRequest>()))
                            .ReturnsAsync((CreateToDoResponse)null);

            var result = await _controller.Create(new CreateToDoRequest());
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Update_ReturnsNoContent_WhenUpdateSuccessful()
        {
            _mockToDoService.Setup(service => service.UpdateAsync(It.IsAny<UpdateToDoRequest>()))
                            .ReturnsAsync(true);

            var result = await _controller.Update(1, new UpdateToDoRequest());
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Update_ReturnsBadRequest_WhenServiceThrowsException()
        {
            _mockToDoService.Setup(service => service.UpdateAsync(It.IsAny<UpdateToDoRequest>()))
                            .ReturnsAsync(false);

            var result = await _controller.Update(1, new UpdateToDoRequest());
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsOk_WhenDeleteSuccessful()
        {
            _mockToDoService.Setup(service => service.DeleteAsync(It.IsAny<int>()))
                            .ReturnsAsync(true);

            var result = await _controller.Delete(1);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsBadRequest_WhenServiceThrowsException()
        {
            _mockToDoService.Setup(service => service.DeleteAsync(It.IsAny<int>()))
                            .ReturnsAsync(false);

            var result = await _controller.Delete(1);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}
