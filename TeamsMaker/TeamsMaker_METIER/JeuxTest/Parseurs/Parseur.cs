using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;

namespace TeamsMaker_METIER.JeuxTest.Parseurs
{
    public class Parseur
    {
        #region Methods
        private Personnage ParserLigne(string ligne)
        {
            string[] morceau = ligne.Split(" ");
            Classe classe = (Classe)Enum.Parse(typeof(Classe), morceau[0]);
            int lvlPrincipal = Int32.Parse(morceau[1]);
            int lvlSecondaire = Int32.Parse(morceau[2]);

            return new Personnage(classe, lvlPrincipal, lvlSecondaire);
        }
        public JeuTest Parser(string nomFichier)
        {
            JeuTest jeuTest = new JeuTest();
            string cheminFichier = Path.Combine(Directory.GetCurrentDirectory(),
            "JeuxTest/Fichiers/" + nomFichier);
            using (StreamReader stream = new StreamReader(cheminFichier))
            {
                string ligne;
                while ((ligne = stream.ReadLine()) != null)
                {
                    jeuTest.AjouterPersonnage(this.ParserLigne(ligne));
                }
            }
            return jeuTest;
        }
        #endregion 
    }
}
