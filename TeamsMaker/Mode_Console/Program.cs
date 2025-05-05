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

            //On récupère le fichier ou il y a 10 000 personnages
            nomfichier = parseur.Parser("DixMille.jt");

            //On choisit d'utilsier l'algorithme glouton
            AlgorithmeGloutonCroissant glouton = new AlgorithmeGloutonCroissant();

            //On utilise la méthode Repartir de l'algorithme glouton (on lance l'algorithme sur le fichier choisi précédemment)
            Repartition repartir = glouton.Repartir(nomfichier);

            //On choisi un probleme' (si on veut en simple, avec ou sans les rôles secondaire) ici on choisi simple
            Probleme probleme = new Probleme();
            probleme = Probleme.SIMPLE;

            //On lance l'évaluation de la repartition sur le problème choisi précédemment
            repartir.LancerEvaluation(probleme);

            //On arrête le calcul de temps
            stopwatch.Stop();
            
            //Afichage du temps, on saute une ligne et on affiche le score de la répartition
            Console.WriteLine(stopwatch);
            Console.WriteLine("\n");
            Console.WriteLine(repartir.Score);


        }
    }
}