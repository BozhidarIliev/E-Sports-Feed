using AutoMapper;
using ESportsFeed.Data.Common.Repositories;
using ESportsFeed.Data.Models;
using ESportsFeed.Services.Models;
using ESportsFeed.Web.DTOs;
using Moq;

namespace ESportsFeed.Web.Test
{
    [TestFixture]
    public class MatchServiceTests
    {
        private MatchService matchService;
        private Mock<IMatchRepository> matchRepositoryMock;
        private Mock<IMapper> mapperMock;

        [SetUp]
        public void SetUp()
        {
            matchRepositoryMock = new Mock<IMatchRepository>();
            mapperMock = new Mock<IMapper>();
            matchService = new MatchService(matchRepositoryMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task GetMatchesStartingInNext24Hours_ShouldReturnMatchesInNext24Hours()
        {
            // Arrange
            var expectedMatches = new List<Data.Models.Match>
            {
                new Data.Models.Match
                {
                    Name = "Match 1",
                    StartDate = DateTime.UtcNow.AddHours(2),
                    Markets = new List<Market>
                    {
                        new Market
                        {
                            Name = "Market 1",
                            Odds = new List<Odd>
                            {
                                new Odd { Name = "Odd 1", Value = 1.5M },
                                new Odd { Name = "Odd 2", Value = 2.0M }
                            }
                        },
                        new Market
                        {
                            Name = "Market 2",
                            Odds = new List<Odd>
                            {
                                new Odd { Name = "Odd 3", Value = 3.0M },
                                new Odd { Name = "Odd 4", Value = 4.0M }
                            }
                        }
                    }
                },
                new Data.Models.Match
                {
                    Name = "Match 2",
                    StartDate = DateTime.UtcNow.AddHours(5),
                    Markets = new List<Market>
                    {
                        new Market
                        {
                            Name = "Market 3",
                            Odds = new List<Odd>
                            {
                                new Odd { Name = "Odd 5", Value = 1.8M },
                                new Odd { Name = "Odd 6", Value = 2.2M}
                            }
                        }
                    }
                },
                new Data.Models.Match
                {
                    Name = "Match 3",
                    StartDate = DateTime.UtcNow.AddHours(10),
                    Markets = new List<Market>
                    {
                        new Market
                        {
                            Name = "Market 4",
                            Odds = new List<Odd>
                            {
                                new Odd { Name = "Odd 7", Value = 1.3M },
                                new Odd { Name = "Odd 8", Value = 1.6M }
                            }
                        },
                        new Market
                        {
                            Name = "Market 5",
                            Odds = new List<Odd>
                            {
                                new Odd { Name = "Odd 9", Value = 2.5M },
                                new Odd { Name = "Odd 10", Value = 3.0M, SpecialBetValue =  2.5M },
                                new Odd { Name = "Odd 11", Value = 3.0M, SpecialBetValue =  2.5M },
                                new Odd { Name = "Odd 12", Value = 3.0M, SpecialBetValue =  3.0M },
                                new Odd { Name = "Odd 13", Value = 3.0M, SpecialBetValue =  3.0M },
                                new Odd { Name = "Odd 14", Value = 3.0M, SpecialBetValue =  3.0M },
                            }
                        }
                    }
                }
            };
            matchRepositoryMock.Setup(repo => repo.All())
        .Returns(expectedMatches.AsQueryable());

            // Mock the mapper to return the expected MatchDTO objects
            var expectedMatchDTOs = expectedMatches.Select(match => new MatchDTO
            {
                Name = match.Name,
                StartDate = match.StartDate,
                ActivePreviewBets = GetActivePreviewBets(match.Markets)
            }).ToList();

            mapperMock.Setup(mapper => mapper.Map<List<MatchDTO>>(It.IsAny<IEnumerable<Data.Models.Match>>()))
                .Returns(expectedMatchDTOs);

            // Act
            var result = matchService.GetMatchesStartingInNext24Hours();

            // Assert
            CollectionAssert.AreEquivalent(expectedMatchDTOs, result);
        }



        [Test]
        public void GetMatchById_WithValidId_ShouldReturnMatchDetailsDTO()
        {
            // Arrange
            var matchId = "1";
            var expectedMatch = new Data.Models.Match { ID = matchId, Name = "Match 1", StartDate = DateTime.UtcNow };
            var expectedMatchDetails = new MatchDetailsDTO { Name = "Match 1", StartDate = DateTime.UtcNow };

            matchRepositoryMock.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(expectedMatch);
            mapperMock.Setup(mapper => mapper.Map<MatchDetailsDTO>(expectedMatch)).Returns(expectedMatchDetails);

            // Act
            var result = matchService.GetMatchById(matchId).Result;

            // Assert
            Assert.That(result, Is.EqualTo(expectedMatchDetails));
        }

        [Test]
        public void GetMatchById_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var matchId = "999";
            Data.Models.Match nullMatch = null;

            matchRepositoryMock.Setup(repo => repo.GetByIdAsync(matchId)).ReturnsAsync(nullMatch);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => matchService.GetMatchById(matchId));
        }
    }
}
