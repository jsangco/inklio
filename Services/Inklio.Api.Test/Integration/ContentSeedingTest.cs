using System.Text.Json;
using Autofac;
using Azure.Core;
using Inklio.Api.Application.Commands.Accounts;
using Inklio.Api.Dependencies;
using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Inklio.Api.Client;

namespace Inklio.Api.Test;

[Trait("api", "integration")]
public class ContentSeedingTest
{
        public static string[] Usernames = new string[]
    {
        "SparklingUnicorn23",
        "CyberNinja42",
        "GalacticExplorer7",
        "MysticalMermaid88",
        "QuantumPioneer19",
        "WanderingSoul55",
        "ElectricPhoenix21",
        "MidnightDreamer33",
        "TechnoWizard77",
        "EnchantedEagle44",
        "PixelWarrior99",
        "CosmicAdventurer12",
        "NeonJellyfish67",
        "DreamyDragonfly25",
        "TimeTraveler63",
        "RainbowSorcerer14",
        "StarryNightSky77",
        "DigitalNomad30",
        "LunarWanderer47",
        "MysticMoonlight22",
        "ShadowWalker55",
        "CodeMasterX88",
        "SereneSnowflake8",
        "AeroSpaceAce13",
        "MagicalMarmoset41",
        "QuantumQuokka99",
        "NeonNebula17",
        "CyberSamurai72",
        "ElectricAardvark3",
        "CrystalCrafter27",
        "EnigmaExplorer46",
        "TechnoTiger84",
        "CelestialCheetah9",
        "PixieWarlock57",
        "WarpDriveWizard22",
        "AuroraBorealis77",
        "TimeLordTrekker61",
        "PlasmaPhoenix14",
        "MysticMonolith33",
        "ShadowSeeker66",
        "BinaryBard37",
        "LunarLionheart4",
        "DreamyDolphin89",
        "QuantumQuasar11",
        "NeonNightingale73",
        "CyberCentaur28",
        "ElectricEchidna6",
        "CrystalChronicle49",
        "EnchantedElk24",
        "nebula_nomad68",
        "mystic_mongoose37",
        "shadow_sorceress95",
        "code_crusaderx11",
        "serene_serpent66",
        "aero_alchemy77",
        "cosmic_cicada42",
        "quantum_qilin18",
        "neon_nomad47",
        "digital_dragonfly62",
        "lunar_larkspur36",
        "techno_toucan11",
        "celestial_chameleon7",
        "pixie_pioneer99",
        "warp_wizardx15",
        "aurora_astronomer29",
        "timeless_traveler84",
        "plasma_peregrine53",
        "mystic_mammoth76",
        "shadow_salamander44",
        "binary_bison22",
        "lunar_lynx17",
        "dreamy_dingo71",
        "quantum_quokka55",
        "neon_nymph79",
        "cyber_coyote38",
        "electric_eagleray39",
        "crystal_coyotex7",
        "enigma_echidna26",
        "techno_traveller53",
        "celestial_centipede9",
        "pixie_python42",
        "warp_whale_watcher17",
    };

    public static string[] Emails = Usernames.Select(i => i + "@mailinator.com").ToArray();

    // public static User[] Users = Enumerable.Range(0, Usernames.Length).Select(i => new User(Usernames[i], Emails[i])).ToArray();

    [Fact]
    public void SeedContent()
    {
        // Create 100 users
        // Randomly create 100 asks from 100 different users
        // Randomly create 100 delivers from 100 different users
        // Randomly create 200 ask comments from 100 different users
        // Randomly create 200 delivery comments from 100 different users
        // Randomly uptvote asks from 100 different users
        // Randomly uptvote deliveries from 100 different users
        // Randomly uptvote asks comments from 100 different users
        // Randomly uptvote deliveries comments from 100 different users
        // Store user actions locally to prevent duplicate
        var user = new Client.User("jace3", "inkliojace3@mailinator.com");
        user.CreateOrLogin();
        user.GetClaims();
        // user.AddAsk(new AskCreateCommand());
    }
}