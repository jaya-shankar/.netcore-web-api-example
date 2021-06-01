using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pokedex.Application.Core.Clients.FunTranslations;
using Pokedex.Application.Core.Clients.FunTranslations.Models;
using Pokedex.Application.Core.Clients.PokeAPI;
using Pokedex.Application.Core.Clients.PokeAPI.Models;
using Pokedex.Application.Core.Entities;
using Pokedex.Application.Core.Services;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Pokedex.Application.Core.Test.Unit.Services
{
    [TestClass]
    public class PokemonServiceTest
    {
        private const string CAVE_HABITAT = "cave";
        private const string ORIGINAL_DESCRIPTION = "This is a\ndescription";
        private const string EXPECTED_DESCRIPTION = "This is a description";
        private const string POKEMON_NAME = "pikachu";
        private const string TRANSLATED_DESCIRPTION = "Translated description, this is.";
        private const string URBAN_HABITAT = "urban";

        private Mock<IFunTranslationsClient> __FunTranslationsClientMock;
        private Mock<IPokeAPIClient> __PokeAPIClientMock;
        private PokemonService __PokemonService;

        private void ArrangeFunTranslationsClientMock(string translation)
        {
            __FunTranslationsClientMock.Setup(client => client.ToShakespeareAsync(It.IsAny<TranslationRequest>())).ReturnsAsync(GetMockedTranslationResponse(translation));
            __FunTranslationsClientMock.Setup(client => client.ToYodaAsync(It.IsAny<TranslationRequest>())).ReturnsAsync(GetMockedTranslationResponse(translation));
        }

        private void ArrangePokeAPIClientMock(Pokemon pokemon, HttpStatusCode statusCode)
        {
            __PokeAPIClientMock.Setup(mock => mock.GetSpeciesAsync(It.IsAny<string>())).ReturnsAsync(() =>
            {
                Mock<IApiResponse<Pokemon>> _Response = new();

                _Response.SetupGet(r => r.StatusCode).Returns(statusCode);
                _Response.SetupGet(r => r.Content).Returns(pokemon);
                _Response.SetupGet(r => r.IsSuccessStatusCode).Returns(statusCode == HttpStatusCode.OK);

                return _Response.Object;
            });
        }

        private static Pokemon CreatePokemon(string name, string habitat, string description, string language, bool isLegendary = false) => new Pokemon
        {
            Name = name,
            Habitat = new PokemonHabitat
            {
                Name = habitat
            },
            Descriptions = description == null ? null : new List<FlavorText>
            {
                new FlavorText
                {
                    Language = new Language
                    {
                        Name = language
                    },
                    Value = description
                }
            },
            IsLegendary = isLegendary
        };

        private IApiResponse<TranslationResponse> GetMockedTranslationResponse(string translation)
        {
            Mock<IApiResponse<TranslationResponse>> _Response = new();

            _Response.SetupGet(r => r.IsSuccessStatusCode).Returns(translation != null);
            _Response.SetupGet(r => r.Content).Returns(new TranslationResponse
            {
                Content = new TranslatedContent
                {
                    TranslatedText = translation
                }
            });

            return _Response.Object;
        }

        [TestInitialize]
        public void Initialize()
        {
            __PokeAPIClientMock = new Mock<IPokeAPIClient>();
            __FunTranslationsClientMock = new Mock<IFunTranslationsClient>();
            __PokemonService = new PokemonService(__PokeAPIClientMock.Object, __FunTranslationsClientMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task PokemonService_GetPokemonAsync_EmptyName_ShouldThrowException() => await __PokemonService.GetPokemonAsync(string.Empty);

        [TestMethod]
        public async Task PokemonService_GetPokemonAsync_Exists_ShouldReturnEnglishDescription()
        {
            ArrangePokeAPIClientMock(CreatePokemon(POKEMON_NAME, CAVE_HABITAT, ORIGINAL_DESCRIPTION, "en"), HttpStatusCode.OK);

            PokemonEntity _Entity = await __PokemonService.GetPokemonAsync(POKEMON_NAME);

            Assert.IsTrue(_Entity.Exists);
            Assert.AreEqual(POKEMON_NAME, _Entity.Name);
            Assert.AreEqual(CAVE_HABITAT, _Entity.Habitat);
            Assert.AreEqual(EXPECTED_DESCRIPTION, _Entity.Description);
        }

        [TestMethod]
        public async Task PokemonService_GetPokemonAsync_NonEnglishDescription_ShouldReturnEmptyString()
        {
            ArrangePokeAPIClientMock(CreatePokemon(POKEMON_NAME, CAVE_HABITAT, ORIGINAL_DESCRIPTION, "fr"), HttpStatusCode.OK);

            PokemonEntity _Entity = await __PokemonService.GetPokemonAsync(POKEMON_NAME);

            Assert.AreEqual(string.Empty, _Entity.Description);
        }

        [TestMethod]
        public async Task PokemonService_GetPokemonAsync_PokemonNotFound_ShouldReturnEmptyEntity()
        {
            ArrangePokeAPIClientMock(null, HttpStatusCode.NotFound);

            PokemonEntity _Entity = await __PokemonService.GetPokemonAsync("bluewaters");

            Assert.IsFalse(_Entity.Exists);
        }

        [TestMethod]
        public async Task PokemonService_GetTranslatedPokemonAsync_CaveHabitat_ShouldApplyYodaTranslation()
        {
            ArrangePokeAPIClientMock(CreatePokemon(POKEMON_NAME, CAVE_HABITAT, ORIGINAL_DESCRIPTION, "en"), HttpStatusCode.OK);
            ArrangeFunTranslationsClientMock(TRANSLATED_DESCIRPTION);

            PokemonEntity _Entity = await __PokemonService.GetTranslatedPokemonAsync(POKEMON_NAME);

            Assert.AreEqual(TRANSLATED_DESCIRPTION, _Entity.Description);

            __FunTranslationsClientMock.Verify(m => m.ToShakespeareAsync(It.IsAny<TranslationRequest>()), Times.Never);
            __FunTranslationsClientMock.Verify(m => m.ToYodaAsync(It.IsAny<TranslationRequest>()), Times.Once);
        }

        [TestMethod]
        public async Task PokemonService_GetTranslatedPokemonAsync_EmptyDescription_ShouldNotCallTheTranslationService()
        {
            ArrangePokeAPIClientMock(CreatePokemon(POKEMON_NAME, CAVE_HABITAT, string.Empty, "fr"), HttpStatusCode.OK);
            ArrangeFunTranslationsClientMock(TRANSLATED_DESCIRPTION);

            PokemonEntity _Entity = await __PokemonService.GetTranslatedPokemonAsync(POKEMON_NAME);

            Assert.IsTrue(_Entity.Exists);
            Assert.AreEqual(POKEMON_NAME, _Entity.Name);

            __FunTranslationsClientMock.Verify(m => m.ToShakespeareAsync(It.IsAny<TranslationRequest>()), Times.Never);
            __FunTranslationsClientMock.Verify(m => m.ToYodaAsync(It.IsAny<TranslationRequest>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task PokemonService_GetTranslatedPokemonAsync_EmptyName_ShouldThrowException() => await __PokemonService.GetTranslatedPokemonAsync(string.Empty);

        [TestMethod]
        public async Task PokemonService_GetTranslatedPokemonAsync_IsLegendary_ShouldApplyYodaTranslation()
        {
            ArrangePokeAPIClientMock(CreatePokemon(POKEMON_NAME, URBAN_HABITAT, ORIGINAL_DESCRIPTION, "en", true), HttpStatusCode.OK);
            ArrangeFunTranslationsClientMock(TRANSLATED_DESCIRPTION);

            PokemonEntity _Entity = await __PokemonService.GetTranslatedPokemonAsync(POKEMON_NAME);

            Assert.AreEqual(TRANSLATED_DESCIRPTION, _Entity.Description);

            __FunTranslationsClientMock.Verify(m => m.ToShakespeareAsync(It.IsAny<TranslationRequest>()), Times.Never);
            __FunTranslationsClientMock.Verify(m => m.ToYodaAsync(It.IsAny<TranslationRequest>()), Times.Once);
        }

        [TestMethod]
        public async Task PokemonService_GetTranslatedPokemonAsync_NeitherCaveNorLegendary_ShouldApplyShakespeareTranslation()
        {
            ArrangePokeAPIClientMock(CreatePokemon(POKEMON_NAME, URBAN_HABITAT, ORIGINAL_DESCRIPTION, "en"), HttpStatusCode.OK);
            ArrangeFunTranslationsClientMock(TRANSLATED_DESCIRPTION);

            PokemonEntity _Entity = await __PokemonService.GetTranslatedPokemonAsync(POKEMON_NAME);

            Assert.AreEqual(TRANSLATED_DESCIRPTION, _Entity.Description);

            __FunTranslationsClientMock.Verify(m => m.ToShakespeareAsync(It.IsAny<TranslationRequest>()), Times.Once);
            __FunTranslationsClientMock.Verify(m => m.ToYodaAsync(It.IsAny<TranslationRequest>()), Times.Never);
        }

        [TestMethod]
        public async Task PokemonService_GetTranslatedPokemonAsync_NotSuccessStatus_ShouldFallbackToOriginalTranslation()
        {
            ArrangePokeAPIClientMock(CreatePokemon(POKEMON_NAME, URBAN_HABITAT, ORIGINAL_DESCRIPTION, "en", true), HttpStatusCode.OK);
            ArrangeFunTranslationsClientMock(null);

            PokemonEntity _Entity = await __PokemonService.GetTranslatedPokemonAsync(POKEMON_NAME);

            Assert.AreEqual(EXPECTED_DESCRIPTION, _Entity.Description);

            __FunTranslationsClientMock.Verify(m => m.ToYodaAsync(It.IsAny<TranslationRequest>()), Times.Once);
        }

        [TestMethod]
        public async Task PokemonService_GetTranslatedPokemonAsync_PokemonNotFound_ShouldReturnEmptyEntity()
        {
            ArrangePokeAPIClientMock(null, HttpStatusCode.NotFound);

            PokemonEntity _Entity = await __PokemonService.GetTranslatedPokemonAsync("bluewaters");

            Assert.IsFalse(_Entity.Exists);
        }
    }
}
