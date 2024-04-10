using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenClaimsService tokenClaimsService, IAsyncEfRepository<User> userRepo, IAsyncEfRepository<Region> regionRepo, IAsyncEfRepository<Municipality> municipalityRepo, AppDbContext context)
        {
            await SeedUserRoles(roleManager);

            if (!context.Users.Any())
            {
                await SeedAdminUser(userRepo, tokenClaimsService);
            }

            if (!context.Municipalities.Any())
            {
                await SeedMunicipalityiesAndRegions(municipalityRepo);
            }
        }

        private static async Task SeedUserRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("Διαχειριστής"));
            await roleManager.CreateAsync(new IdentityRole("Γεωπόνος"));
            await roleManager.CreateAsync(new IdentityRole("Καλλιεργητής"));
        }

        private static async Task SeedAdminUser(IAsyncEfRepository<User> userRepo, ITokenClaimsService tokenClaimsService)
        {
            await userRepo.AddAsync(new User()
            {
                ApplicationUserId = await tokenClaimsService.CreateIdentityUser("test@gmail.com", "admin", "Διαχειριστής"),
                FirstName = "test",
                LastName = "test",
                Phone = "test",
                Email = "test@gmail.com",
                UserName = "test@gmail.com",
                Role = "Διαχειριστής"
            });
        }

        private static async Task SeedMunicipalityiesAndRegions(IAsyncEfRepository<Municipality> municipalityRepo)
        {
            var regionMacedonia = "ΠΕΡΙΦΕΡΕΙΑ ΑΝΑΤΟΛΙΚΗΣ ΜΑΚΕΔΟΝΙΑΣ ΚΑΙ ΘΡΑΚΗΣ";
            List<string> municipaliyMacedoniaList = new List<string> { "ΔΗΜΟΣ ΔΟΞΑΤΟΥ", "ΔΗΜΟΣ ΔΡΑΜΑΣ", "ΔΗΜΟΣ ΚΑΤΩ ΝΕΥΡΟΚΟΠΙΟΥ", "ΔΗΜΟΣ ΠΑΡΑΝΕΣΤΙΟΥ", "ΔΗΜΟΣ ΠΡΟΣΟΤΣΑΝΗΣ", "ΔΗΜΟΣ ΑΛΕΞΑΝΔΡΟΥΠΟΛΗΣ", "ΔΗΜΟΣ ΔΙΔΥΜΟΤΕΙΧΟΥ", "ΔΗΜΟΣ ΟΡΕΣΤΙΑΔΑΣ", "ΔΗΜΟΣ ΣΑΜΟΘΡΑΚΗΣ", "ΔΗΜΟΣ ΣΟΥΦΛΙΟΥ", "ΔΗΜΟΣ ΘΑΣΟΥ", "ΔΗΜΟΣ ΚΑΒΑΛΑΣ", "ΔΗΜΟΣ ΝΕΣΤΟΥ", "ΔΗΜΟΣ ΠΑΓΓΑΙΟΥ", "ΔΗΜΟΣ ΑΒΔΗΡΩΝ", "ΔΗΜΟΣ ΜΥΚΗΣ", "ΔΗΜΟΣ ΞΑΝΘΗΣ", "ΔΗΜΟΣ ΤΟΠΕΙΡΟΥ", "ΔΗΜΟΣ ΑΡΡΙΑΝΩΝ", "ΔΗΜΟΣ ΙΑΣΜΟΥ", "ΔΗΜΟΣ ΚΟΜΟΤΗΝΗΣ", "ΔΗΜΟΣ ΜΑΡΩΝΕΙΑΣ – ΣΑΠΩΝ" };

            var regionAttica = "ΠΕΡΙΦΕΡΕΙΑ ΑΤΤΙΚΗΣ";
            List<string> municipaliyAtticaList = new List<string> { "ΔΗΜΟΣ ΑΧΑΡΝΩΝ", "ΔΗΜΟΣ ΒΑΡΗΣ – ΒΟΥΛΑΣ – ΒΟΥΛΙΑΓΜΕΝΗΣ", "ΔΗΜΟΣ ΔΙΟΝΥΣΟΥ", "ΔΗΜΟΣ ΚΡΩΠΙΑΣ", "ΔΗΜΟΣ ΛΑΥΡΕΩΤΙΚΗΣ", "ΔΗΜΟΣ ΜΑΡΑΘΩΝΟΣ", "ΔΗΜΟΣ ΜΑΡΚΟΠΟΥΛΟΥ ΜΕΣΟΓΑΙΑΣ", "ΔΗΜΟΣ ΠΑΙΑΝΙΑΣ", "ΔΗΜΟΣ ΠΑΛΛΗΝΗΣ", "ΔΗΜΟΣ ΡΑΦΗΝΑΣ – ΠΙΚΕΡΜΙΟΥ", "ΔΗΜΟΣ ΣΑΡΩΝΙΚΟΥ", "ΔΗΜΟΣ ΣΠΑΤΩΝ – ΑΡΤΕΜΙΔΟΣ", "ΔΗΜΟΣ ΩΡΩΠΟΥ", "ΔΗΜΟΣ ΑΓΙΑΣ ΠΑΡΑΣΚΕΥΗΣ", "ΔΗΜΟΣ ΑΜΑΡΟΥΣΙΟΥ", "ΔΗΜΟΣ ΒΡΙΛΗΣΣΙΩΝ", "ΔΗΜΟΣ ΗΡΑΚΛΕΙΟΥ", "ΔΗΜΟΣ ΚΗΦΙΣΙΑΣ", "ΔΗΜΟΣ ΛΥΚΟΒΡΥΣΗΣ – ΠΕΥΚΗΣ", "ΔΗΜΟΣ ΜΕΤΑΜΟΡΦΩΣΕΩΣ", "ΔΗΜΟΣ ΝΕΑΣ ΙΩΝΙΑΣ", "ΔΗΜΟΣ ΠΑΠΑΓΟΥ – ΧΟΛΑΡΓΟΥ", "ΔΗΜΟΣ ΠΕΝΤΕΛΗΣ", "ΔΗΜΟΣ ΦΙΛΟΘΕΗΣ – ΨΥΧΙΚΟΥ", "ΔΗΜΟΣ ΧΑΛΑΝΔΡΙΟΥ", "ΔΗΜΟΣ ΑΣΠΡΟΠΥΡΓΟΥ", "ΔΗΜΟΣ ΕΛΕΥΣΙΝΑΣ", "ΔΗΜΟΣ ΜΑΝΔΡΑΣ – ΕΙΔΥΛΛΙΑΣ", "ΔΗΜΟΣ ΜΕΓΑΡΕΩΝ", "ΔΗΜΟΣ ΦΥΛΗΣ", "ΔΗΜΟΣ ΑΓΙΑΣ ΒΑΡΒΑΡΑΣ", "ΔΗΜΟΣ ΑΓΙΩΝ ΑΝΑΡΓΥΡΩΝ – ΚΑΜΑΤΕΡΟΥ", "ΔΗΜΟΣ ΑΙΓΑΛΕΩ", "ΔΗΜΟΣ ΙΛΙΟΥ", "ΔΗΜΟΣ ΠΕΡΙΣΤΕΡΙΟΥ", "ΔΗΜΟΣ ΠΕΤΡΟΥΠΟΛΕΩΣ", "ΔΗΜΟΣ ΧΑΪΔΑΡΙΟΥ", "ΔΗΜΟΣ ΑΘΗΝΑΙΩΝ", "ΔΗΜΟΣ ΒΥΡΩΝΟΣ", "ΔΗΜΟΣ ΓΑΛΑΤΣΙΟΥ", "ΔΗΜΟΣ ΔΑΦΝΗΣ – ΥΜΗΤΤΟΥ", "ΔΗΜΟΣ ΖΩΓΡΑΦΟΥ", "ΔΗΜΟΣ ΗΛΙΟΥΠΟΛΕΩΣ", "ΔΗΜΟΣ ΚΑΙΣΑΡΙΑΝΗΣ", "ΔΗΜΟΣ ΦΙΛΑΔΕΛΦΕΙΑΣ – ΧΑΛΚΗΔΟΝΟΣ", "ΔΗΜΟΣ ΑΓΚΙΣΤΡΙΟΥ", "ΔΗΜΟΣ ΑΙΓΙΝΑΣ", "ΔΗΜΟΣ ΚΥΘΗΡΩΝ", "ΔΗΜΟΣ ΠΟΡΟΥ", "ΔΗΜΟΣ ΣΑΛΑΜΙΝΟΣ", "ΔΗΜΟΣ ΣΠΕΤΣΩΝ", "ΔΗΜΟΣ ΤΡΟΙΖΗΝΙΑΣ", "ΔΗΜΟΣ ΥΔΡΑΣ", "ΔΗΜΟΣ ΑΓΙΟΥ ΔΗΜΗΤΡΙΟΥ", "ΔΗΜΟΣ ΑΛΙΜΟΥ", "ΔΗΜΟΣ ΓΛΥΦΑΔΑΣ", "ΔΗΜΟΣ ΕΛΛΗΝΙΚΟΥ – ΑΡΓΥΡΟΥΠΟΛΗΣ", "ΔΗΜΟΣ ΚΑΛΛΙΘΕΑΣ", "ΔΗΜΟΣ ΜΟΣΧΑΤΟΥ – ΤΑΥΡΟΥ", "ΔΗΜΟΣ ΝΕΑΣ ΣΜΥΡΝΗΣ", "ΔΗΜΟΣ ΠΑΛΑΙΟΥ ΦΑΛΗΡΟΥ", "ΔΗΜΟΣ ΚΕΡΑΤΣΙΝΙΟΥ – ΔΡΑΠΕΤΣΩΝΑΣ", "ΔΗΜΟΣ ΚΟΡΥΔΑΛΛΟΥ", "ΔΗΜΟΣ ΝΙΚΑΙΑΣ – ΑΓΙΟΥ ΙΩΑΝΝΗ ΡΕΝΤΗ", "ΔΗΜΟΣ ΠΕΙΡΑΙΩΣ", "ΔΗΜΟΣ ΠΕΡΑΜΑΤΟΣ" };

            var regionAegean = "ΠΕΡΙΦΕΡΕΙΑ ΒΟΡΕΙΟΥ ΑΙΓΑΙΟΥ";
            List<string> municipaliyAegeanList = new List<string> { "ΔΗΜΟΣ ΙΚΑΡΙΑΣ", "ΔΗΜΟΣ ΦΟΥΡΝΩΝ ΚΟΡΣΕΩΝ", "ΔΗΜΟΣ ΛΕΣΒΟΥ", "ΔΗΜΟΣ ΑΓΙΟΥ ΕΥΣΤΡΑΤΙΟΥ", "ΔΗΜΟΣ ΛΗΜΝΟΥ", "ΔΗΜΟΣ ΣΑΜΟΥ", "ΔΗΜΟΣ ΟΙΝΟΥΣΣΩΝ", "ΔΗΜΟΣ ΧΙΟΥ", "ΔΗΜΟΣ ΨΑΡΩΝ" };

            var regionWestGreece = "ΠΕΡΙΦΕΡΕΙΑ ΔΥΤΙΚΗΣ ΕΛΛΑΔΑΣ";
            List<string> municipaliyWestGreeceList = new List<string> { "ΔΗΜΟΣ ΑΓΡΙΝΙΟΥ", "ΔΗΜΟΣ ΑΚΤΙΟΥ – ΒΟΝΙΤΣΑΣ", "ΔΗΜΟΣ ΑΜΦΙΛΟΧΙΑΣ", "ΔΗΜΟΣ ΘΕΡΜΟΥ", "ΔΗΜΟΣ ΙΕΡΑΣ ΠΟΛΗΣ ΜΕΣΟΛΟΓΓΙΟΥ", "ΔΗΜΟΣ ΝΑΥΠΑΚΤΙΑΣ", "ΔΗΜΟΣ ΞΗΡΟΜΕΡΟΥ", "ΔΗΜΟΣ ΑΙΓΙΑΛΕΙΑΣ", "ΔΗΜΟΣ ΔΥΤΙΚΗΣ ΑΧΑΪΑΣ", "ΔΗΜΟΣ ΕΡΥΜΑΝΘΟΥ", "ΔΗΜΟΣ ΚΑΛΑΒΡΥΤΩΝ", "ΔΗΜΟΣ ΠΑΤΡΕΩΝ", "ΔΗΜΟΣ ΑΝΔΡΑΒΙΔΑΣ – ΚΥΛΛΗΝΗΣ", "ΔΗΜΟΣ ΑΝΔΡΙΤΣΑΙΝΑΣ – ΚΡΕΣΤΕΝΩΝ", "ΔΗΜΟΣ ΑΡΧΑΙΑΣ ΟΛΥΜΠΙΑΣ", "ΔΗΜΟΣ ΖΑΧΑΡΩΣ", "ΔΗΜΟΣ ΗΛΙΔΑΣ", "ΔΗΜΟΣ ΠΗΝΕΙΟΥ", "ΔΗΜΟΣ ΠΥΡΓΟΥ" };

            var regionWestMacedonia = "ΠΕΡΙΦΕΡΕΙΑ ΔΥΤΙΚΗΣ ΜΑΚΕΔΟΝΙΑΣ";
            List<string> municipaliyWestMacedoniaList = new List<string> { "ΔΗΜΟΣ ΓΡΕΒΕΝΩΝ", "ΔΗΜΟΣ ΔΕΣΚΑΤΗΣ", "ΔΗΜΟΣ ΚΑΣΤΟΡΙΑΣ", "ΔΗΜΟΣ ΝΕΣΤΟΡΙΟΥ", "ΔΗΜΟΣ ΟΡΕΣΤΙΔΟΣ", "ΔΗΜΟΣ ΒΟΪΟΥ", "ΔΗΜΟΣ ΕΟΡΔΑΙΑΣ", "ΔΗΜΟΣ ΚΟΖΑΝΗΣ", "ΔΗΜΟΣ ΣΕΡΒΙΩΝ – ΒΕΛΒΕΝΤΟΥ", "ΔΗΜΟΣ ΑΜΥΝΤΑΙΟΥ", "ΔΗΜΟΣ ΠΡΕΣΠΩΝ", "ΔΗΜΟΣ ΦΛΩΡΙΝΑΣ" };

            var regionIpirus = "ΠΕΡΙΦΕΡΕΙΑ ΗΠΕΙΡΟΥ";
            List<string> municipaliyIpirusList = new List<string> { "ΔΗΜΟΣ ΑΡΤΑΙΩΝ", "ΔΗΜΟΣ ΓΕΩΡΓΙΟΥ ΚΑΡΑΪΣΚΑΚΗ", "ΔΗΜΟΣ ΚΕΝΤΡΙΚΩΝ ΤΖΟΥΜΕΡΚΩΝ", "ΔΗΜΟΣ ΝΙΚΟΛΑΟΥ ΣΚΟΥΦΑ", "ΔΗΜΟΣ ΗΓΟΥΜΕΝΙΤΣΑΣ", "ΔΗΜΟΣ ΣΟΥΛΙΟΥ", "ΔΗΜΟΣ ΦΙΛΙΑΤΩΝ", "ΔΗΜΟΣ ΒΟΡΕΙΩΝ ΤΖΟΥΜΕΡΚΩΝ", "ΔΗΜΟΣ ΔΩΔΩΝΗΣ", "ΔΗΜΟΣ ΖΑΓΟΡΙΟΥ", "ΔΗΜΟΣ ΖΙΤΣΑΣ", "ΔΗΜΟΣ ΙΩΑΝΝΙΤΩΝ", "ΔΗΜΟΣ ΚΟΝΙΤΣΑΣ", "ΔΗΜΟΣ ΜΕΤΣΟΒΟΥ", "ΔΗΜΟΣ ΠΩΓΩΝΙΟΥ", "ΔΗΜΟΣ ΖΗΡΟΥ", "ΔΗΜΟΣ ΠΑΡΓΑΣ", "ΔΗΜΟΣ ΠΡΕΒΕΖΑΣ" };

            var regionThessaly = "ΠΕΡΙΦΕΡΕΙΑ ΘΕΣΣΑΛΙΑΣ";
            List<string> municipaliyThesallyList = new List<string> { "ΔΗΜΟΣ ΑΡΓΙΘΕΑΣ", "ΔΗΜΟΣ ΚΑΡΔΙΤΣΑΣ", "ΔΗΜΟΣ ΛΙΜΝΗΣ ΠΛΑΣΤΗΡΑ", "ΔΗΜΟΣ ΜΟΥΖΑΚΙΟΥ", "ΔΗΜΟΣ ΠΑΛΑΜΑ", "ΔΗΜΟΣ ΣΟΦΑΔΩΝ", "ΔΗΜΟΣ ΑΓΙΑΣ", "ΔΗΜΟΣ ΕΛΑΣΣΟΝΑΣ", "ΔΗΜΟΣ ΚΙΛΕΛΕΡ", "ΔΗΜΟΣ ΛΑΡΙΣΑΙΩΝ", "ΔΗΜΟΣ ΤΕΜΠΩΝ", "ΔΗΜΟΣ ΤΥΡΝΑΒΟΥ", "ΔΗΜΟΣ ΦΑΡΣΑΛΩΝ", "ΔΗΜΟΣ ΑΛΜΥΡΟΥ", "ΔΗΜΟΣ ΒΟΛΟΥ", "ΔΗΜΟΣ ΖΑΓΟΡΑΣ – ΜΟΥΡΕΣΙΟΥ", "ΔΗΜΟΣ ΝΟΤΙΟΥ ΠΗΛΙΟΥ", "ΔΗΜΟΣ ΡΗΓΑ ΦΕΡΑΙΟΥ", "ΔΗΜΟΣ ΑΛΟΝΝΗΣΟΥ", "ΔΗΜΟΣ ΣΚΙΑΘΟΥ", "ΔΗΜΟΣ ΣΚΟΠΕΛΟΥ", "ΔΗΜΟΣ ΚΑΛΑΜΠΑΚΑΣ", "ΔΗΜΟΣ ΠΥΛΗΣ", "ΔΗΜΟΣ ΤΡΙΚΚΑΙΩΝ", "ΔΗΜΟΣ ΦΑΡΚΑΔΟΝΑΣ", "ΠΕΡΙΦΕΡΕΙΑ ΙΟΝΙΩΝ ΝΗΣΩΝ", "ΔΗΜΟΣ ΖΑΚΥΝΘΟΥ", "ΔΗΜΟΣ ΙΘΑΚΗΣ", "ΔΗΜΟΣ ΚΕΡΚΥΡΑΣ", "ΔΗΜΟΣ ΠΑΞΩΝ", "ΔΗΜΟΣ ΚΕΦΑΛΟΝΙΑΣ", "ΔΗΜΟΣ ΛΕΥΚΑΔΑΣ", "ΔΗΜΟΣ ΜΕΓΑΝΗΣΙΟΥ" };

            var regionCentralMacedonia = "ΠΕΡΙΦΕΡΕΙΑ ΚΕΝΤΡΙΚΗΣ ΜΑΚΕΔΟΝΙΑΣ";
            List<string> municipaliyCentralMacedoniaList = new List<string> { "ΔΗΜΟΣ ΑΛΕΞΑΝΔΡΕΙΑΣ", "ΔΗΜΟΣ ΒΕΡΟΙΑΣ", "ΔΗΜΟΣ ΝΑΟΥΣΑΣ", "ΔΗΜΟΣ ΑΜΠΕΛΟΚΗΠΩΝ – ΜΕΝΕΜΕΝΗΣ", "ΔΗΜΟΣ ΒΟΛΒΗΣ", "ΔΗΜΟΣ ΔΕΛΤΑ", "ΔΗΜΟΣ ΘΕΡΜΑΪΚΟΥ", "ΔΗΜΟΣ ΘΕΡΜΗΣ", "ΔΗΜΟΣ ΘΕΣΣΑΛΟΝΙΚΗΣ", "ΔΗΜΟΣ ΚΑΛΑΜΑΡΙΑΣ", "ΔΗΜΟΣ ΚΟΡΔΕΛΙΟΥ – ΕΥΟΣΜΟΥ", "ΔΗΜΟΣ ΛΑΓΚΑΔΑ", "ΔΗΜΟΣ ΝΕΑΠΟΛΗΣ – ΣΥΚΕΩΝ", "ΔΗΜΟΣ ΠΑΥΛΟΥ ΜΕΛΑ", "ΔΗΜΟΣ ΠΥΛΑΙΑΣ – ΧΟΡΤΙΑΤΗ", "ΔΗΜΟΣ ΧΑΛΚΗΔΟΝΟΣ", "ΔΗΜΟΣ ΩΡΑΙΟΚΑΣΤΡΟΥ", "ΔΗΜΟΣ ΚΙΛΚΙΣ", "ΔΗΜΟΣ ΠΑΙΟΝΙΑΣ", "ΔΗΜΟΣ ΑΛΜΩΠΙΑΣ", "ΔΗΜΟΣ ΕΔΕΣΣΑΣ", "ΔΗΜΟΣ ΠΕΛΛΑΣ", "ΔΗΜΟΣ ΣΚΥΔΡΑΣ", "ΔΗΜΟΣ ΔΙΟΥ – ΟΛΥΜΠΟΥ", "ΔΗΜΟΣ ΚΑΤΕΡΙΝΗΣ", "ΔΗΜΟΣ ΠΥΔΝΑΣ – ΚΟΛΙΝΔΡΟΥ", "ΔΗΜΟΣ ΑΜΦΙΠΟΛΗΣ", "ΔΗΜΟΣ ΒΙΣΑΛΤΙΑΣ", "ΔΗΜΟΣ ΕΜΜΑΝΟΥΗΛ ΠΑΠΠΑ", "ΔΗΜΟΣ ΗΡΑΚΛΕΙΑΣ", "ΔΗΜΟΣ ΝΕΑΣ ΖΙΧΝΗΣ", "ΔΗΜΟΣ ΣΕΡΡΩΝ", "ΔΗΜΟΣ ΣΙΝΤΙΚΗΣ", "ΔΗΜΟΣ ΑΡΙΣΤΟΤΕΛΗ", "ΔΗΜΟΣ ΚΑΣΣΑΝΔΡΑΣ", "ΔΗΜΟΣ ΝΕΑΣ ΠΡΟΠΟΝΤΙΔΑΣ", "ΔΗΜΟΣ ΠΟΛΥΓΥΡΟΥ", "ΔΗΜΟΣ ΣΙΘΩΝΙΑΣ" };

            var regionCrete = "ΠΕΡΙΦΕΡΕΙΑ ΚΡΗΤΗΣ";
            List<string> municipaliyCreteList = new List<string> { "ΔΗΜΟΣ ΑΡΧΑΝΩΝ – ΑΣΤΕΡΟΥΣΙΩΝ", "ΔΗΜΟΣ ΒΙΑΝΝΟΥ", "ΔΗΜΟΣ ΓΟΡΤΥΝΑΣ", "ΔΗΜΟΣ ΗΡΑΚΛΕΙΟΥ", "ΔΗΜΟΣ ΜΑΛΕΒΙΖΙΟΥ", "ΔΗΜΟΣ ΜΙΝΩΑ ΠΕΔΙΑΔΑΣ", "ΔΗΜΟΣ ΦΑΙΣΤΟΥ", "ΔΗΜΟΣ ΧΕΡΣΟΝΗΣΟΥ", "ΔΗΜΟΣ ΑΓΙΟΥ ΝΙΚΟΛΑΟΥ", "ΔΗΜΟΣ ΙΕΡΑΠΕΤΡΑΣ", "ΔΗΜΟΣ ΟΡΟΠΕΔΙΟΥ ΛΑΣΙΘΙΟΥ", "ΔΗΜΟΣ ΣΗΤΕΙΑΣ", "ΔΗΜΟΣ ΑΓΙΟΥ ΒΑΣΙΛΕΙΟΥ", "ΔΗΜΟΣ ΑΜΑΡΙΟΥ", "ΔΗΜΟΣ ΑΝΩΓΕΙΩΝ", "ΔΗΜΟΣ ΜΥΛΟΠΟΤΑΜΟΥ", "ΔΗΜΟΣ ΡΕΘΥΜΝΗΣ", "ΔΗΜΟΣ ΑΠΟΚΟΡΩΝΟΥ", "ΔΗΜΟΣ ΓΑΥΔΟΥ", "ΔΗΜΟΣ ΚΑΝΤΑΝΟΥ – ΣΕΛΙΝΟΥ", "ΔΗΜΟΣ ΚΙΣΣΑΜΟΥ", "ΔΗΜΟΣ ΠΛΑΤΑΝΙΑ", "ΔΗΜΟΣ ΣΦΑΚΙΩΝ", "ΔΗΜΟΣ ΧΑΝΙΩΝ" };

            var regionSouthAegean = "ΠΕΡΙΦΕΡΕΙΑ ΝΟΤΙΟΥ ΑΙΓΑΙΟΥ";
            List<string> municipaliySouthAegeanList = new List<string> { "ΔΗΜΟΣ ΑΝΔΡΟΥ", "ΔΗΜΟΣ ΑΝΑΦΗΣ", "ΔΗΜΟΣ ΘΗΡΑΣ", "ΔΗΜΟΣ ΙΗΤΩΝ", "ΔΗΜΟΣ ΣΙΚΙΝΟΥ", "ΔΗΜΟΣ ΦΟΛΕΓΑΝΔΡΟΥ", "ΔΗΜΟΣ ΑΓΑΘΟΝΗΣΙΟΥ", "ΔΗΜΟΣ ΑΣΤΥΠΑΛΑΙΑΣ", "ΔΗΜΟΣ ΚΑΛΥΜΝΙΩΝ", "ΔΗΜΟΣ ΛΕΙΨΩΝ", "ΔΗΜΟΣ ΛΕΡΟΥ", "ΔΗΜΟΣ ΠΑΤΜΟΥ", "ΔΗΜΟΣ ΚΑΡΠΑΘΟΥ", "ΔΗΜΟΣ ΚΑΣΟΥ", "ΔΗΜΟΣ ΚΕΑΣ", "ΔΗΜΟΣ ΚΥΘΝΟΥ", "ΔΗΜΟΣ ΚΩ", "ΔΗΜΟΣ ΝΙΣΥΡΟΥ", "ΔΗΜΟΣ ΚΙΜΩΛΟΥ", "ΔΗΜΟΣ ΜΗΛΟΥ", "ΔΗΜΟΣ ΣΕΡΙΦΟΥ", "ΔΗΜΟΣ ΣΙΦΝΟΥ", "ΔΗΜΟΣ ΜΥΚΟΝΟΥ", "ΔΗΜΟΣ ΑΜΟΡΓΟΥ", "ΔΗΜΟΣ ΝΑΞΟΥ ΚΑΙ ΜΙΚΡΩΝ ΚΥΚΛΑΔΩΝ", "ΔΗΜΟΣ ΑΝΤΙΠΑΡΟΥ", "ΔΗΜΟΣ ΠΑΡΟΥ", "ΔΗΜΟΣ ΜΕΓΙΣΤΗΣ", "ΔΗΜΟΣ ΡΟΔΟΥ", "ΔΗΜΟΣ ΣΥΜΗΣ", "ΔΗΜΟΣ ΤΗΛΟΥ", "ΔΗΜΟΣ ΧΑΛΚΗΣ", "ΔΗΜΟΣ ΣΥΡΟΥ – ΕΡΜΟΥΠΟΛΗΣ", "ΔΗΜΟΣ ΤΗΝΟΥ" };

            var municipalityList = new List<Municipality>();

            GetMunicipalityList(municipalityList, municipaliyMacedoniaList, new Region() { Name = regionMacedonia });
            GetMunicipalityList(municipalityList, municipaliyAtticaList, new Region() { Name = regionAttica });
            GetMunicipalityList(municipalityList, municipaliyAegeanList, new Region() { Name = regionAegean });
            GetMunicipalityList(municipalityList, municipaliyWestGreeceList, new Region() { Name = regionWestGreece });
            GetMunicipalityList(municipalityList, municipaliyWestMacedoniaList, new Region() { Name = regionWestMacedonia });
            GetMunicipalityList(municipalityList, municipaliyIpirusList, new Region() { Name = regionIpirus });
            GetMunicipalityList(municipalityList, municipaliyThesallyList, new Region() { Name = regionThessaly });
            GetMunicipalityList(municipalityList, municipaliyCentralMacedoniaList, new Region() { Name = regionCentralMacedonia });
            GetMunicipalityList(municipalityList, municipaliyCreteList, new Region() { Name = regionCrete });
            GetMunicipalityList(municipalityList, municipaliySouthAegeanList, new Region() { Name = regionSouthAegean });
            await municipalityRepo.AddRangeAsync(municipalityList);
        }

        private static List<Municipality> GetMunicipalityList(List<Municipality> municipalities, List<string> municipalitiesNamesList, Region region)
        {
            foreach (var item in municipalitiesNamesList)
            {
                municipalities.Add(new Municipality()
                {
                    Name = item,
                    Region = region
                });
            }

            return municipalities;
        }
    }

}
