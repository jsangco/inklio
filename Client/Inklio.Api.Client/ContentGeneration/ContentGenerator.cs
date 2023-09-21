namespace Inklio.Api.Client;

public class ContentGenerator
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

    private readonly IReadOnlyList<User> users;

    public ContentGenerator(IEnumerable<string> usernames)
    {
        this.users = usernames.Select(u => new User(u)).ToList();
    }

    public async Task SeedContent(string sampleImagePath, CancellationToken cancellationToken = default)
    {
        await this.LoginAllUsersAsync();
        await this.CreateAsksAsync();
        await this.CreateDeliveriesAsync(sampleImagePath);
        await this.CreateAskComments();
        await this.CreateDeliveryComments();
    }

    private Task LoginAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var logins = this.users.Select(u => u.CreateOrLoginAsync(cancellationToken)).ToArray();
        return Task.WhenAll(logins);
    }

    private async Task CreateAsksAsync(CancellationToken cancellationToken = default)
    {
        var askCreates = this.users
            .Zip(SampleAsks.AskCreates, (u, a) => (u, a))
            .Select(ua => ua.u.AddAskAsync(ua.a, cancellationToken));
        foreach (var task in askCreates)
        {
            await task;
        }
    }

    private async Task CreateDeliveriesAsync(string sampleImagePath, CancellationToken cancellationToken = default)
    {
        byte[] imageBytes = await File.ReadAllBytesAsync(sampleImagePath, cancellationToken);

        var rand = new Random(1);
        var user = this.users.First();
        var askRequest = await user.GetAsksAsync(null, cancellationToken);
        List<Ask> asks = new List<Ask>(100);
        while (askRequest.NextLink != null)
        {
            asks.AddRange(askRequest.Value);
            askRequest = await user.GetAsksAsync(askRequest.NextLink.ToString(), cancellationToken);
        }
        var createDeliveries = SampleDeliveries.DeliveryCreates.Select(delivery =>
        {
            delivery.Images = new byte[][] { imageBytes };
            var ask = asks[rand.Next(asks.Count)];
            var user = this.users[rand.Next(this.users.Count)];
            if (string.Equals(ask.CreatedBy, user.Username, StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask; // Skip over deliveries for an ask created by the same user.
            }

            return user.AddDeliveryAsync(delivery, ask.Id, cancellationToken);
        });

        foreach (var task in createDeliveries)
        {
            await task;
        }
    }

    private async Task CreateAskComments(CancellationToken cancellationToken = default)
    {
        var rand = new Random(0);
        Ask[] asks = (await this.users.First().GetAsksAsync(null, cancellationToken)).Value.ToArray();
        var createComments = SampleComments.AskCommentCreate.Select(comment =>
        {
            var ask = asks[rand.Next(asks.Length)];
            var user = this.users[rand.Next(this.users.Count)];
            return user.AddCommentAsync(comment, ask.Id, cancellationToken);
        });
        foreach(var task in createComments)
        {
            await task;
        }
    }

    private async Task CreateDeliveryComments(CancellationToken cancellationToken = default)
    {
        var rand = new Random(0);
        Ask[] asks = (await this.users.First().GetAsksAsync(null, cancellationToken)).Value.ToArray();
        var createComments = SampleComments.DeliveryCommentCreate.Select(comment =>
        {
            var ask = asks[rand.Next(asks.Length)];
            var user = this.users[rand.Next(this.users.Count)];
            while (ask.Deliveries.Any() == false)
            {
                ask = asks[rand.Next(asks.Length)];
            }
            var delivery = ask.Deliveries.ToArray()[rand.Next(ask.Deliveries.Count())];
            return user.AddCommentAsync(comment, ask.Id, delivery.Id, cancellationToken);
        });

        foreach(var task in createComments)
        {
            await task;
        }

    }
}