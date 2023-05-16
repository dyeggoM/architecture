using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TEST.Architectures.UnitOfWork.Controllers;
using TEST.Architectures.UnitOfWork.Entities;
using TEST.Architectures.UnitOfWork.Interfaces;

namespace TEST.Architectures.UnitOfWork.UnitTesting
{
    [TestFixture]
    public class DbModelsControllerTests
    {
        private static DbModel _model = new DbModel(){Id = 1, Name = "a"};
        private Task<IEnumerable<DbModel>> _dbModels = Task.Run(()=>new List<DbModel>
            {
                _model
            }.AsEnumerable());
        private Mock<IUnitOfWork> _unitOfWork;
        private DbModelsController _controller;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _controller = new DbModelsController(_unitOfWork.Object);
        }

        [Test]
        public void GetDbModel_WhenCalled_ReturnsOk()
        {
            _unitOfWork.Setup(uow => uow.dbModelRepository.GetAll()).Returns(_dbModels);
            var result = _controller.GetDbModel();
            Assert.That(result.Result,Is.TypeOf<OkObjectResult>());
        }
        [Test]
        public void GetDbModel_WhenGetDbModelError_ReturnsBadRequest()
        {
            _unitOfWork.Setup(uow => uow.dbModelRepository.GetAll()).Throws<Exception>();
            var result = _controller.GetDbModel();
            Assert.That(result.Result,Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void GetDbModelById_WhenIdExists_ReturnsOk()
        {
            _unitOfWork.Setup(uow => uow.dbModelRepository.Find(1)).Returns(Task.Run(()=>_model));
            var result = _controller.GetDbModel(1);
            Assert.That(result.Result,Is.TypeOf<OkObjectResult>());
        }
        [Test]
        public void GetDbModelById_WhenIdDoesNotExist_ReturnsNoContent()
        {
            _unitOfWork.Setup(uow => uow.dbModelRepository.Find(1)).Returns(Task.Run(()=>_model));
            var result = _controller.GetDbModel(2);
            Assert.That(result.Result,Is.TypeOf<NotFoundResult>());
        }
        [Test]
        public void GetDbModelById_WhenGetDbModelByIdError_ReturnsBadRequest()
        {
            _unitOfWork.Setup(uow => uow.dbModelRepository.Find(1)).Throws<Exception>();
            var result = _controller.GetDbModel(1);
            Assert.That(result.Result,Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void PutDbModel_WhenIdAndDbModelIdDoesNotMatch_UpdateIsNotCalledAndSaveChangesIsNotCalledAndReturnsBadrequest()
        {
            var result = _controller.PutDbModel(2,_model);
            _unitOfWork.Verify(s=>s.dbModelRepository.Update(2,_model),Times.Never);
            _unitOfWork.Verify(s=>s.SaveChangesAsync(),Times.Never);
            Assert.That(result.Result,Is.TypeOf<BadRequestObjectResult>());
        }
        [Test]
        public void PutDbModel_WhenDbModelExists_UpdateIsCalledAndSaveChangesIsCalledAndReturnsNoContent()
        {
            _unitOfWork.Setup(uow => uow.dbModelRepository.Update(1, _model)).Returns(Task.Run(() => { }));
            var result = _controller.PutDbModel(1, _model);
            _unitOfWork.Verify(s=>s.dbModelRepository.Update(1,_model),Times.Once);
            _unitOfWork.Verify(s=>s.SaveChangesAsync(),Times.Once);
            Assert.That(result.Result, Is.TypeOf<NoContentResult>());
        }
        [Test]
        public void PutDbModel_WhenUpdateError_ReturnsBadRequest()
        {
            _unitOfWork.Setup(uow => uow.dbModelRepository.Update(1,_model)).Throws<Exception>();
            var result = _controller.PutDbModel(1,_model);
            Assert.That(result.Result,Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void PostDbModel_WhenDbModelIsOk_AddIsCalledAndSaveChangesIsCalledAndReturnsOk()
        {
            _unitOfWork.Setup(uow => uow.dbModelRepository.Add(_model)).Returns(Task.Run(()=>_model));
            var result = _controller.PostDbModel(_model);
            _unitOfWork.Verify(s=>s.dbModelRepository.Add(_model),Times.Once);
            _unitOfWork.Verify(s=>s.SaveChangesAsync(),Times.Once);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
        [Test]
        public void PostDbModel_WhenAddError_SaveChangesIsNotCalledAndReturnsBadRequest()
        {
            _unitOfWork.Setup(uow => uow.dbModelRepository.Add(_model)).Throws<Exception>();
            var result = _controller.PostDbModel(_model);
            _unitOfWork.Verify(s=>s.SaveChangesAsync(),Times.Never);
            Assert.That(result.Result,Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void DeleteDbModel_WhenDbModelExists_DeleteIsCalledAndSaveChangesIsCalledAndReturnsNoContent()
        {
            _unitOfWork.Setup(uow => uow.dbModelRepository.Delete(1)).Returns(Task.Run(()=>_model));
            var result = _controller.DeleteDbModel(1);
            _unitOfWork.Verify(s=>s.dbModelRepository.Delete(1),Times.Once);
            _unitOfWork.Verify(s=>s.SaveChangesAsync(),Times.Once);
            Assert.That(result.Result, Is.TypeOf<NoContentResult>());
        }
        [Test]
        public void DeleteDbModel_WhenDeleteError_SaveChangesIsNotCalledAndReturnsBadRequest()
        {
            _unitOfWork.Setup(uow => uow.dbModelRepository.Delete(1)).Throws<Exception>();
            var result = _controller.DeleteDbModel(1);
            _unitOfWork.Verify(s=>s.SaveChangesAsync(),Times.Never);
            Assert.That(result.Result,Is.TypeOf<BadRequestObjectResult>());
        }

    }
}