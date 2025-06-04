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
            nomfichier = parseur.Parser("DonjonClassique_10000.jt");

            //On choisit d'utilsier l'algorithme glouton
            //lgorithmeGloutonCroissant glouton = new AlgorithmeGloutonCroissant();

            //Première heuristique de niveau 2
            //Heuristique1_niveau2 heuristique = new Heuristique1_niveau2();

            //N-Swap
            //N_Swap nswap = new N_Swap();

            //N-Swap améliroer
            //NSwapAmeliore nswameliorer = new NSwapAmeliore();
            //Moyenne des rôles
            AlgorithmeMoyenneNiveau3_role_simple moyenneNiveau3_Role_Simple = new AlgorithmeMoyenneNiveau3_role_simple();

            //On utilise la méthode Repartir de l'algorithme choisi (on lance l'algorithme sur le fichier choisi précédemment)
            Repartition repartir = moyenneNiveau3_Role_Simple.Repartir(nomfichier);

            //On choisi un probleme (si on veut en simple, avec ou sans les rôles secondaire)
            Probleme probleme = new Probleme();
            probleme = Probleme.ROLESECONDAIRE;

            //On lance l'évaluation de la repartition sur le problème choisi précédemment
            repartir.LancerEvaluation(probleme);

            //On arrête le calcul de temps
            stopwatch.Stop();
            
            //Afichage du temps, on saute une ligne et on affiche le score de la répartition
            Console.WriteLine($"Le temps écoulé est de : {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine();
            Console.WriteLine($"Le score est de : {repartir.Score}");
            Console.WriteLine();
            Console.WriteLine($"Nompbre de Personnages sans équipe : {repartir.PersonnagesSansEquipe.Count()}"); //utilie pour les niveaux 2 et 3 ou il y a des personnages sans équipe quand on manque de roles


        }
    }
}