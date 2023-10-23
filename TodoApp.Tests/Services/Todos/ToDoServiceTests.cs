using AutoMapper;
using Moq;
using TodoApp.API.Data.ToDos;
using TodoApp.API.Entities.Commons.Operations;
using TodoApp.API.Entities.ToDos.DTOs;
using TodoApp.API.Entities.ToDos;
using TodoApp.API.Services.ToDos;
using TodoApp.API.Entities.Enums;

namespace TodoApp.Tests.Services.Todos
{
    [TestFixture]
    public class ToDoServiceTests
    {
        private Mock<IToDoRespository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private ToDoService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IToDoRespository>();
            _mockMapper = new Mock<IMapper>();
            _service = new ToDoService(_mockRepository.Object, _mockMapper.Object);
        }

        [Test]
        public async Task CreateAsync_ValidRequest_CreatesToDo()
        {
            // Arrange
            var request = new CreateToDoRequest 
            {
                Name = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now,
                Status = ToDoStatus.NotStarted,
                Priority = ToDoPriority.High
            };

            var mappedToDo = new ToDo 
            {
                Name = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now,
                Status = ToDoStatus.NotStarted,
                Priority = ToDoPriority.High
            };
            var createdToDo = new ToDo 
            { 
                Id = 1,
                Name = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now,
                Status = ToDoStatus.NotStarted,
                Priority = ToDoPriority.High
            };
            var response = new CreateToDoResponse();

            _mockMapper.Setup(m => m.Map<ToDo>(request)).Returns(mappedToDo);
            _mockRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync(createdToDo);
            _mockMapper.Setup(m => m.Map<CreateToDoResponse>(createdToDo)).Returns(response);

            // Act
            var result = await _service.CreateAsync(request);

            // Assert
            _mockRepository.Verify(r => r.Add(mappedToDo), Times.Once);
            _mockRepository.Verify(r => r.SaveAll(), Times.Once);
            Assert.AreEqual(response, result);
        }

        [Test]
        public async Task DeleteAsync_ValidId_DeletesToDo()
        {
            // Arrange
            var toDoId = 1;
            var toDo = new ToDo 
            {
                Id = 1,
                Name = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now,
                Status = ToDoStatus.NotStarted,
                Priority = ToDoPriority.High
            };
            _mockRepository.Setup(r => r.GetAsync(toDoId)).ReturnsAsync(toDo);
            _mockRepository.Setup(r => r.SaveAll()).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(toDoId);

            // Assert
            _mockRepository.Verify(r => r.Delete(toDo), Times.Once);
            _mockRepository.Verify(r => r.SaveAll(), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAsync_ValidId_ReturnsToDo()
        {
            var toDoId = 1;
            var toDo = new ToDo 
            {
                Id = toDoId,
                Name = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now,
                Status = ToDoStatus.NotStarted,
                Priority = ToDoPriority.High
            };
            var response = new GetToDoResponse
            {
                Id = 1,
                Name = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now,
                Status = ToDoStatus.NotStarted,
                Priority = ToDoPriority.High
            };

            _mockRepository.Setup(r => r.GetAsync(toDoId)).ReturnsAsync(toDo);
            _mockMapper.Setup(m => m.Map<GetToDoResponse>(toDo)).Returns(response);

            var result = await _service.GetAsync(toDoId);

            Assert.AreEqual(response, result);
        }

        [Test]
        public async Task ListAsync_ValidRequest_ReturnsToDos()
        {
            var request = new ListToDoRequest
            {
                PageNumber = 1,
                PageSize = 10,
                OrderBy = BaseListOrderBy.Descending,
                OrderByName = "CreatedDate"
            };
            var pagedList = new PagedList<ToDo>(new List<ToDo>(), 1, 1, 1);
            var responseList = new PagedList<ListToDoResponse>(new List<ListToDoResponse>(), 1, 1, 1);

            _mockRepository.Setup(r => r.ListAsync(request)).ReturnsAsync(pagedList);
            _mockMapper.Setup(m => m.Map<PagedList<ListToDoResponse>>(pagedList)).Returns(responseList);

            var result = await _service.ListAsync(request);

            Assert.AreEqual(responseList, result);
        }

        [Test]
        public async Task UpdateAsync_ValidRequest_UpdatesToDo()
        {
            var request = new UpdateToDoRequest 
            {
                Id = 1,
                Name = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now,
                Status = ToDoStatus.NotStarted,
                Priority = ToDoPriority.High
            };
            var mappedToDo = new ToDo
            {
                Id = 1,
                Name = "Task 1",
                Description = "Description 1",
                DueDate = DateTime.Now,
                Status = ToDoStatus.NotStarted,
                Priority = ToDoPriority.High
            };

            _mockMapper.Setup(m => m.Map<ToDo>(request)).Returns(mappedToDo);
            _mockRepository.Setup(r => r.UpdateAsync(mappedToDo)).ReturnsAsync(true);

            var result = await _service.UpdateAsync(request);

            _mockRepository.Verify(r => r.UpdateAsync(mappedToDo), Times.Once);
            Assert.IsTrue(result);
        }
    }
}