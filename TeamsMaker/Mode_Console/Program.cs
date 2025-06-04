using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.JeuxTest.Parseurs;
using TeamsMaker_METIER.Algorithmes.Realisations;
using TeamsMaker_METIER.JeuxTest;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TeamsMaker_METIER.Problemes;


namespace TeamsMaker
{
    public class Program
    {
        static void Main(string[] args)
        {
            //On créer un stopwatch afin de calculer le temps que met l'algorithme
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Parseur parseur = new Parseur();
            JeuTest nomfichier = new JeuTest();

            //On récupère le fichier ou il y a le nombre de personnages
            nomfichier = parseur.Parser("Dixmille.jt");

            //On choisit d'utilsier l'algorithme glouton
            //lgorithmeGloutonCroissant glouton = new AlgorithmeGloutonCroissant();

            //Première heuristique de niveau 2
            //Heuristique1_niveau2 heuristique = new Heuristique1_niveau2();

            //N-Swap
            //N_Swap nswap = new N_Swap();

            //moyenne niveau 3
            //AlgorithmeMoyenneNiveau3_role_simple moyenneNiveau3 = new AlgorithmeMoyenneNiveau3_role_simple();
            AlgorithmeMyenne3_role_et_sousrole moyenneNiveau3 = new AlgorithmeMyenne3_role_et_sousrole();
            //N-Swap améliroer
            //NSwapAmeliore nswameliorer = new NSwapAmeliore();

            //On utilise la méthode Repartir de l'algorithme choisi (on lance l'algorithme sur le fichier choisi précédemment)
            Repartition repartir = moyenneNiveau3.Repartir(nomfichier);

            //On choisi un probleme (si on veut en simple, avec ou sans les rôles secondaire)
            Probleme probleme = new Probleme();
            probleme = Probleme.ROLESECONDAIRE; // Problème avec rôle secondaire

            //On lance l'évaluation de la repartition sur le problème choisi précédemment
            repartir.LancerEvaluation(probleme);

            //On arrête le calcul de temps
            stopwatch.Stop();

            //Afichage du temps, on saute une ligne et on affiche le score de la répartition
            Console.WriteLine($"Le temps est de : {moyenneNiveau3.TempsExecution} ms");
            Console.WriteLine();
            Console.WriteLine($"Le score est de : {repartir.Score}");
            Console.WriteLine($"Il reste {repartir.PersonnagesSansEquipe.Count()} personnages sans équipe"); // On affiche le nombre de personnages sans équipe utile pour le problème de niveau 3

        }
    }
}