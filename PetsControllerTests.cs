using AutoMapper;
using GURU_CoMM;
using GURU_CoMM.Controllers;
using GURU_CoMM.DTOs;
using GURU_CoMM.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace GURU_CoMM_UnitaryTests
{
    [TestClass]
    public class PetsControllerTests
    {
        private ApplicationDbContext constructDbContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName).Options;

            var dbContext = new ApplicationDbContext(options);
            return dbContext;
        }

        [TestMethod]
        public void Get_Specific_Pet()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var dbContext = constructDbContext(databaseName);
            var pet1 = new Pet() { Name = "Luffy" };
            var pet2 = new Pet() { Name = "Ronny" };
            var pet3 = new Pet() { Name = "Senpai" };
            dbContext.AddRange(pet1, pet2, pet3);
            dbContext.SaveChanges();

            var pet4 = new Pet() { Name = "Rolo" };
            var mock = new Mock<IMapper>();
            mock.Setup(x => x.Map<PetDto>(pet4))
                .Returns(new PetDto
                    {
                        Id = pet4.Id,
                        Name = pet4.Name
                    });

            var dbContext2 = constructDbContext(databaseName);
            var petsConroller = new PetsController(dbContext2, mock.Object);
            long petId = 1;

            // Act
            var result = petsConroller.Get(petId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, petId);
        }
    }
}
