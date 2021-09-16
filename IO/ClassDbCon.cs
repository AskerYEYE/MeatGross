using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace IO
{
    /// <summary>
    /// Denne Class benyttes til kommunikation mellem et C# projekt og
    /// en given Sql-Server og udpeget database.
    /// Metoderne i denne class, kan kun tilgås gennem nedarvning
    /// </summary>
    public class ClassDbCon
    {
        // Fields der benyttes i div. metoder i denne Class.
        private string _connectionString;
        public SqlConnection con;
        private SqlCommand _command;

        /// <summary>
        /// Default Constructor.
        /// her bliver _connectionString initialiseret til en gyldig connection string med data omkring URL til Sql-Serveren og
        /// information omkring hvilken database der skal benyttes på serveren.
        /// Yderligere info omkring indhold Connectionstring findes her: 
        /// https://www.connectionstrings.com/sql-server/
        /// </summary>
        public ClassDbCon()
        {
            _connectionString = @"Server=(localdb)\MSSQLLocaldb;Database=MeatGross;Trusted_Connection=True;";
            con = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Overloaded Constructor.
        /// Har samme Funktion som den default contructor men skal modtage parameter med angivelse af connection til Sql-Serveren via en
        /// connectionstring.
        /// Yderligere info omkring indhold Connectionstring findes her: 
        /// https://www.connectionstrings.com/sql-server/
        /// </summary>
        /// <param name="inConString">string</param>
        public ClassDbCon(string inConString)
        {
            _connectionString = inConString;
            con = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Metode som kan benyttes til at definere hvilken Sql-Server og/eller
        /// hvilken Database der skal oprettes forbindelse til.
        /// metoden kan kaldes efter behov og vil overrule den SqlConnection som blev oprettet i en af de to constructors.
        /// Yderligere info omkring indhold Connectionstring findes her: 
        /// https://www.connectionstrings.com/sql-server/
        /// </summary>
        /// <param name="inConString">string</param>
        protected void SetCon(string inConString)
        {
            _connectionString = inConString;
            con = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Denne metode åbner forbindelsen til databasen.
        /// Den undersøger om alle betingelser er opfyldt for at åbne forbindelsen, inden den åbnes.
        /// Hvis betingelserne ikke er opfyldt, vil den prøve at håndtere de mest almindelige fejl og mangler.
        /// </summary>
        protected void OpenDB()
        {
            try
            {
                if (this.con != null && con.State == ConnectionState.Closed) // Undersøger om instansen con er initialiseret og at der ikke er en åben forbindelse i forvejen
                {
                    con.Open();//Åbner forbindelsen til DB.
                }
                else // Hvis betingelserne i 'if' ikke er opfyldt
                {
                    if (con.State == ConnectionState.Open) // Undersøger om fejlen skylden at der er en åben forbindelse i forvejen
                    {
                        // Hvis ja - Lukker forbindelsen og kalder 'sig selv' igen for at åbne forbindelsen.
                        CloseDB();
                        OpenDB();
                    }
                    else // Hvis det ikke var pga. af en åben forbindelse så må det være pga manglende initialisering af 'con'.
                    {
                        con = new SqlConnection(_connectionString); // inintialisere 'con' med den angivne connectionstring
                        OpenDB(); // kalder 'sig selv' igen for at åbne forbindelsen
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Denne metode lukker forbindelsen til DB.
        /// </summary>
        protected void CloseDB()
        {
            try
            {
                con.Close(); // Lukker forbindelsen
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Denne metode har til formål, at udføre de handlinger i databasen, som ikke kræver at der returneres et resultatsæt.
        /// Metoden vil dog altid returnere en intiger værdi der angiver om handlingen gik godt eller skidt.
        /// Returneres: -1 er handlingen ikke blevet udført
        /// Returneres: Et tal fra 0 til N, indikerer det at udtrykket kunne eksekveres og angiver hvor mange datasæt der
        /// blev påvirket
        /// </summary>
        /// <param name="sqlQuery">string</param>
        /// <returns>int</returns>
        protected int ExecuteNonQuery(string sqlQuery)
        {
            int res = 0;
            _command = new SqlCommand(sqlQuery, con); // Her initialiseres instansen af SqlCommand med parmetre string sqlQuery og SqlConnection con

            try
            {
                OpenDB(); // Åbner forbindelse til DB
                res = _command.ExecuteNonQuery(); // Her kaldes databasen og den givne query eksekveres
            }
            catch (SqlException ex) // Håndtere de exceptions (fejl) der måtte opståe under kommunikationen med databasen
            {
                throw ex;
            }
            finally // Ved angivelse 'finally' sikre jeg, at det der står i 'finally' altid bliver udført, uanset om koden kunne eksekveres med eller uden fejl
            {
                CloseDB();// Lukker forbindelsen
            }

            return res;
        }

        /// <summary>        
        /// Denne metode skal håndtere forespørgelser til databasen som skal returnere et resultatsæt. 
        /// Det resultatsæt der modtages fra DB, konverteres over i en collection af typen DataTable
        /// </summary> 
        /// <param name="sqlQuery">string</param> 
        /// <returns>DataTable</returns>
        protected DataTable DbReturnDataTable(string sqlQuery)
        {
            DataTable dtRes = new DataTable();
            try
            {
                OpenDB();
                using (_command = new SqlCommand(sqlQuery, con)) // Her initialiseres instansen af SqlCommand med parameterne string query og SqlConnection con
                {
                    using (var adapter = new SqlDataAdapter(_command)) // Her foretages kaldet til databasen ved, at der oprettes en ny instans af en SqlDataAdapter. Resultatet overføres til en abstrakt datatype.
                    {
                        adapter.Fill(dtRes); // Her overføres data fra den abstrakte datatype til den DataTable metoden skal returnere
                    }
                }
            }
            catch (SqlException ex) // Håndtere de exceptions (fejl) der måtte opstå under kommunikationen med databasen
            {
                throw ex;
            }
            finally // Ved angivelse 'finally' sikre jeg, at det der står i 'finally' altid bliver udført, uanset om koden kunne eksekveres med eller uden fejl            {
            {
                CloseDB();
            }


            return dtRes;
        }


        /// <summary>
        /// Denne metode skal håndtere forespørgelser til databasen som skal returnere en tekststreng.
        /// </summary>       
        /// <param name="sqlQuery">string</param>        
        /// <returns>string</returns>
        protected string DbReturnString(string sqlQuery)
        {
            string res = "";
            bool foundOne = false;

            try
            {
                OpenDB(); // Åbner forbindelsen til databasen
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    // Opretter en ny instans af SqlCommand med parameterne sqlQuery og con,
                    // som indeholder henholdsvis min sql forspørgelse og information omkring
                    // hvilken database data skal hentes fra.
                {
                    SqlDataReader reader = cmd.ExecuteReader();// Her eksekveres forespørgelsen på databasen og svaret gemmes i reader som er af datatypen
                                                               // SqlDataReader der har samme egenskaber som en StreamReader, altså egenskaber der gør den
                                                               // egnet til at modtage og holde en stream af tekst
                    
                    while (reader.Read()) // Hvis reader har modtaget et resultat fra databasen, skal den udføre koden i while loopet
                    {
                        res = reader.GetString(0); // Læser teksten fra reader og indsætter den i res.
                        foundOne = true; // Bolsk værdi, der angiver at der er modtaget et resultat
                    }
                    if (!foundOne) // Hvis der ikke findes et resultat i databasen, skal der returneres en tom tekststreng.
                    {
                        res = "No data found!";
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB();
            }


            return res;
        }

        /// <summary>
        /// Denne metode skal håndtere forespørgelser til databasen som skal returnere et resultatsæt.        
        /// Forespørgelsen skal foretages gennem en StoredProcedure på SqlServeren.        
        /// Det resultatsæt der modtages fra DB, konverteres over i en collection af typen DataTable
        /// </summary>
        /// <param name="inCommand">SqlCommand</param>
        /// <returns>DataTable</returns>
        protected DataTable MakeCallToStoredProcedure(SqlCommand inCommand)
        {
            DataTable dtRes = new DataTable();

            try
            {
                OpenDB();// Åbner forbindelsen til databasen
                using (SqlDataAdapter adapter = new SqlDataAdapter(inCommand)) // Her intialiseres en instans af SqlDataAdapter med 
                                                                               //værdien i inCommand
                {
                    adapter.Fill(dtRes); // Her overføres data fra adapter til den DataTable, metoden skal returnere.
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB(); // Lukker forbindelsen til databasen
            }


            return dtRes;
        }
    }
}
