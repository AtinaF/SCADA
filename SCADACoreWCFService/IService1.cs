using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Web.Services.Description;
using System.Web;

namespace SCADACoreWCFService
{
    /// <summary>
    /// this explains entire project I will create
    /// Katedra za automatsko upravljanje
//    Predmetni projekat
//Softver nadzorno-upravljačkih sistema
//Implementirati SCADA sistem koji podržava sledeće funkcionalnosti:
//- Dodavanje i uklanjanje analognih i digitalnih tagova sa sledećim osobinama:
//DI(digital input) DO(digital output) AI(analog input) AO(analog output)
//- tag name(id)
//- description
//- driver
//- I/O address
//- scan time
//- on/off scan
//- tag name(id)
//- description
//- I/O address
//- initial value
//- tag name(id)
//- description
//- driver
//- I/O address
//- scan time
//- alarms
//- on/off scan
//- low limit
//- high limit
//- units
//- tag name(id)
//- description
//- I/O address
//- initial value
//- low limit
//- high limit
//- units
//- Registraciju i prijavljivanje korisnika za korišćenje DatabaseManager-a.
//- Upisivanje vrednosti izlaznih tagova i prikaz njihovih trenutnih vrednosti preko DatabaseManager
//aplikacije.
//- Uključivanje i isključivanje skeniranja ulaznih tagova (on/off scan).
//- Prikaz trenutnih vrednosti ulaznih tagova sistema preko Trending aplikacije.
//- Čitanje i pisanje konfiguracije sistema iz/u fajl scadaConfig.xml pri pokretanju/zaustavljanju
//SCADA sistema.U konfiguracionom fajlu se uvek mora naći najsvežija konfiguracija
//sistema - voditi računa o izuzecima i nasilnom gašenju SCADA aplikacije.
//Softverska arhitektura sistema definisana je na sledeći način:
//Katedra za automatsko upravljanje
//Opis:
//- Database Manager preko korisničkog interfejsa omogućava dodavanje/uklanjanje tagova
//na serveru, uključivanje/isključivanje skeniranja(on-off scan) ulaznih tagova, upisivanje
//vrednosti izlaznih tagova, prikaz trenutnih vrednosti izlaznih tagova, registraciju i prijavljivanje/
//odjavljivanje korisnika(korisnički podaci se čuvaju u bazi podataka).
//- Trending App prikazuje vrednosti ulaznih(on scan) tagova u sistemu.
//- SCADA Core predstavlja jezgro SCADA sistema i implementira interfejse preko kojih
//je omogućena server-client komunikacija sa ostalim komponentama u sistemu(kreirati
//posebnu servisnu klasu za svaku klijentsku aplikaciju). SCADA Core sadrži komponente
//Simulation Driver i Tag Processing.Simulation Driver omogućava generisanje
//predefinisanih signala(sinus, kosinus, rampa) na unapred definisanim I/O adresama(Simulation
//Driver kreirati kao Class Library projekat). Tag Processing omogućava pravovremeno
//očitavanje vrednosti tagova sa određenih I/O adresa i generiše neophodne događaje
//za njihov prikaz u Trending aplikaciji.
//Nadograditi sistem tako da podržava sledeće funkcionalnosti:
//- Povezivanje (pretplatu) sistema na neki Real-Time Unit (publisher ).
//- Čuvanje (perzistenciju) vrednosti tagova u bazi podataka.
//- Dodavanje i uklanjanje alarma za analogne ulaze.Alarmi imaju sledeća svojstva: tip (low,
//high), prioritet (1,2,3), graničnu vrednost(prag) i ime veličine na koju je vezan alarm.
//- Ispis informacija o alarmima koji se dese u fajl alarmsLog.txt, kao i u bazu podataka.
//- Prikaz alarma koji se dese u sistemu preko Alarm Display klijenta.Alarmi n-tog prioriteta
//se prikazuju n puta zaredom.
//- Čitanje/pisanje konfiguracije alarma iz/u fajl alarmConfig.xml (ili već postojeći scadaConfig.
//xml) pri pokretanju/zaustavljanju SCADA sistema.U konfiguracionom fajlu se uvek
//mora naći najsvežija konfiguracija alarma – voditi računa o izuzecima i nasilnom gašenju
//SCADA aplikacije.
//- Prikaz različitih vrsta izveštaja preko Report Manager klijenta:
//◦ Svi alarmi koji su se desili u određenom vremenskom periodu(sortiranje: prioritet,
//vreme).
//◦ Svi alarmi određenog prioriteta(sortiranje: vreme).
//◦ Sve vrednosti tagova koje su dospele na servis u određenom vremenskom periodu
//(sortiranje: vreme).
//◦ Poslednja vrednost svih AI tagova(sortiranje: vreme).
//◦ Poslednja vrednost svih DI tagova(sortiranje: vreme).
//◦ Sve vrednosti taga sa određenim identifikatorom(sortiranje: vrednosti).
//Opis:
//- Real-Time Unit(RTU) imitira merni uređaj na terenu, koji vrši očitavanje vrednosti
//(jedne) “stvarne” veličine i šalje podatke o toj veličini na servis.RTU ima svoj identifikator,
//gornju i donju granicu za(nasumične) vrednosti koje šalje, kao i adresu Real-Time
//Driver-a na koju će slati pomenute vrednosti(ova adresa je jedinstvena za svaki RTU).
//Ove opcije se unose ručno prilikom pokretanja uređaja/aplikacije.Poruke sa svakog RTU
//(ima ih više) se digitalno potpisuju i proveravaju na servisu pre upisivanja u bazu ili slanja
//ostalim WCF klijentima.
//Katedra za automatsko upravljanje
//- Database Manager dodatno omogućava definisanje alarma za veličine.
//- Alarm Display preko interfejsa ispisuje na konzoli sve alarme koji se dese u sistemu
//zajedno sa tipom alarma, vremenom aktivacije alarma i imenom veličine nad kojom se
//desio alarm.
//- Report Manager preko jednostavnog menija omogućava prikaz gorepomenutih izveštaja.
//- SCADA Core predstavlja jezgro SCADA sistema. Nova verzija jezgra sadrži i Real-
//Time Driver, koji omogućava upisivanje vrednosti pristiglih sa RT uređaja na određenu
//adresu, kao i njihovo očitavanje.
//Softverska arhitektura sistema definisana je na sledeći način:
//Napomena: Komunikaciju u sesijama, instanciranje i konkurentnost servisa definisati po želji, uz
//fokus na jednostavnost i čitljivost rešenja.
    /// </summary>




    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
