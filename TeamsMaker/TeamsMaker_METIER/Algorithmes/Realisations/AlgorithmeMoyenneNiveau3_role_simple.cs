using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgorithmeMoyenneNiveau3_role_simple : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            //création du timer pour mesurer le temps d'exécution de l'algorithme
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Repartition repartition = new Repartition(jeuTest);
            List<Personnage> personnages = repartition.Personnages.ToList();

            //on va trirer les personnages par class principale
            List<Personnage> dps = new List<Personnage>();
            List<Personnage> tank = new List<Personnage>();
            List<Personnage> support = new List<Personnage>();

            foreach (Personnage personnage in personnages)
            {
                if (personnage.RolePrincipal == Personnages.Classes.Role.DPS)
                {
                    dps.Add(personnage);
                }
                else if (personnage.RolePrincipal == Personnages.Classes.Role.TANK)
                {
                    tank.Add(personnage);
                }
                else if (personnage.RolePrincipal == Personnages.Classes.Role.SUPPORT)
                {
                    support.Add(personnage);
                }
            }
            
            Boolean continuer = true; //On continue tant qu'on a des personnages à répartir dans des équipes
            Probleme probleme = new Probleme(); //On crée le problème à évaluer
            probleme = Probleme.ROLESECONDAIRE; //On choisit le problème simple
            while (continuer)
            {
                //on commence par crée une équipe
                Equipe equipe = new Equipe();
                //on comence par ajouter un tank
                Personnage personnageChoisi = null;
                double meilleurecart = int.MaxValue;
                double moyenne = 0; //On va calculer la moyenne des niveaux des personnages de l'équipe
                if (tank.Count > 0)
                {
                    foreach(Personnage personnage in tank)
                    {
                        int nouvelleecart = Math.Abs(personnage.LvlPrincipal - 50); 
                        if (nouvelleecart < meilleurecart) //On cherche le tank le plus proche de 50
                        {
                            meilleurecart = nouvelleecart;
                            personnageChoisi = personnage;
                        }
                    }
                    equipe.AjouterMembre(personnageChoisi); //On ajoute le tank à l'équipe
                    moyenne = personnageChoisi.LvlPrincipal; //On initialise la moyenne avec le niveau du tank
                    tank.Remove(personnageChoisi); //On retire le tank de la liste des personnages disponibles
                }
                else
                {
                    //si on n'a pas de tank, on cherche un personnage avec un rôle secondaire de tank
                    Boolean tankTrouve = false; //On va chercher un personnage avec un rôle secondaire de tank
                    foreach (Personnage personnage in dps)
                        {
                        if (personnage.RoleSecondaire == Personnages.Classes.Role.TANK) //On cherche un personnage qui a un rôle secondaire de tank
                        {
                            tankTrouve = true; //On a trouvé un personnage avec un rôle secondaire de tank
                            double nouvelleecart = Math.Abs(personnage.LvlSecondaire - 50); 
                            if (nouvelleecart < meilleurecart) //On cherche le personnage qui à la moyenne le plus proche de 50
                            {
                                meilleurecart = nouvelleecart;
                                personnageChoisi = personnage;
                            }
                        }
                        if (tankTrouve)
                        {
                            equipe.AjouterMembre(personnageChoisi); //On ajoute le personnage à l'équipe
                            moyenne = personnageChoisi.LvlSecondaire; //On initialise la moyenne avec le niveau du personnage
                            dps.Remove(personnageChoisi); //On retire le personnage de la liste des personnages disponibles
                            break; //On sort de la boucle car on a trouvé un personnage avec un rôle secondaire de tank
                        }
                        else
                        {
                            foreach(Personnage personnage1 in support)
                            {
                                if (personnage1.RoleSecondaire == Personnages.Classes.Role.TANK) //On cherche un personnage qui a un rôle secondaire de tank
                                {
                                    tankTrouve = true; //On a trouvé un personnage avec un rôle secondaire de tank
                                    double nouvelleecart2 = Math.Abs(personnage1.LvlSecondaire - 50); 
                                    if (nouvelleecart2 < meilleurecart) //On cherche le personnage qui à la moyenne le plus proche de 50
                                    {
                                        meilleurecart = nouvelleecart2;
                                        personnageChoisi = personnage1;
                                    }
                                }
                            }
                            if (tankTrouve)
                            {
                                equipe.AjouterMembre(personnageChoisi); //On ajoute le personnage à l'équipe
                                moyenne = personnageChoisi.LvlSecondaire; //On initialise la moyenne avec le niveau du personnage
                                support.Remove(personnageChoisi); //On retire le personnage de la liste des personnages disponibles
                            }
                            else
                            {
                                continuer = false;
                            }
                        }
                    }

                }


                //On ajoute un support
                if (support.Count > 0 & continuer)
                {
                    personnageChoisi = null;
                    meilleurecart = int.MaxValue;
                    foreach (Personnage personnage in support)
                    {
                        double nouvllemoyenne = ((moyenne + personnage.LvlPrincipal) / 2); //On calcule la nouvelle moyenne si on ajoute le personnage
                        double nouvelleecart = Math.Abs(nouvllemoyenne - 50); 
                        if (nouvelleecart < meilleurecart) //On cherche le support qui à la moyenne le plus proche de 50
                        {
                            meilleurecart = nouvelleecart;
                            personnageChoisi = personnage;
                        }
                    }
                    equipe.AjouterMembre(personnageChoisi); //On ajoute le support à l'équipe
                    moyenne = (moyenne + personnageChoisi.LvlPrincipal) / 2; //On met à jour la moyenne
                    support.Remove(personnageChoisi); //On retire le support de la liste des personnages disponibles
                }
                else
                {
                    //on va vérifier les rôles secondaires des personnages tank
                    personnageChoisi = null;
                    meilleurecart = int.MaxValue;
                    bool supportTrouve = false;
                    foreach (Personnage personnage in tank)
                    {
                        if (personnage.RoleSecondaire == Personnages.Classes.Role.SUPPORT) //On cherche un tank qui a un rôle secondaire de support
                            {
                                supportTrouve = true; //On a trouvé un tank qui a un rôle secondaire de support
                            double nouvllemoyenne = ((moyenne + personnage.LvlSecondaire) / 2); //On calcule la nouvelle moyenne si on ajoute le personnage
                                double nouvelleecart = Math.Abs(nouvllemoyenne - 50); 
                                if (nouvelleecart < meilleurecart) //On cherche le tank qui à la moyenne le plus proche de 50
                            {
                                    meilleurecart = nouvelleecart;
                                    personnageChoisi = personnage;
                                }
                            }
                    }
                    if (supportTrouve)
                    {
                        equipe.AjouterMembre(personnageChoisi); //On ajoute le tank à l'équipe
                        moyenne = (moyenne + personnageChoisi.LvlSecondaire) / 2; //On met à jour la moyenne
                        tank.Remove(personnageChoisi); //On retire le tank de la liste des personnages disponibles
                    }
                    else
                    {
                        //on n'a pas trouvé de tank avec un rôle secondaire de support, donc on va chercher dans les dps
                        foreach (Personnage personnage in dps)
                        {
                            if (personnage.RoleSecondaire == Personnages.Classes.Role.SUPPORT) //On cherche un DPS qui a un rôle secondaire de support
                            {
                                supportTrouve = true; //On a trouvé un DPS qui a un rôle secondaire de support
                                double nouvllemoyenne = ((moyenne + personnage.LvlSecondaire) / 2); //On calcule la nouvelle moyenne si on ajoute le personnage
                                double nouvelleecart = Math.Abs(nouvllemoyenne - 50);
                                if (nouvelleecart < meilleurecart) //On cherche le DPS qui à la moyenne le plus proche de 50
                                {
                                    meilleurecart = nouvelleecart;
                                    personnageChoisi = personnage;
                                }
                            }
                        }
                        if (supportTrouve)
                        {
                            equipe.AjouterMembre(personnageChoisi); //On ajoute le DPS à l'équipe
                            moyenne = (moyenne + personnageChoisi.LvlSecondaire) / 2; //On met à jour la moyenne
                            dps.Remove(personnageChoisi); //On retire le DPS de la liste des personnages disponibles
                        }
                        else
                        {
                            continuer = false; //Si on n'a pas trouvé de support, on arrête la répartition
                        }
                    }
                }

                //On ajoute 2 DPS
                if (dps.Count > 1 & continuer)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        personnageChoisi = null;
                        meilleurecart = int.MaxValue;
                        foreach (Personnage personnage in dps)
                        {
                            double nouvllemoyenne = ((moyenne + personnage.LvlPrincipal) / (equipe.Membres.Count()+1)); //On calcule la nouvelle moyenne si on ajoute le personnage
                            double nouvelleecart = Math.Abs(nouvllemoyenne - 50); 
                            if (nouvelleecart < meilleurecart) //On cherche le DPS le plus proche de 50
                            {
                                meilleurecart = nouvelleecart;
                                personnageChoisi = personnage;
                            }
                        }
                        equipe.AjouterMembre(personnageChoisi); //On ajoute le DPS à l'équipe
                        moyenne = (moyenne + personnageChoisi.LvlPrincipal) / (equipe.Membres.Count()); //On met à jour la moyenne
                        dps.Remove(personnageChoisi); //On retire le DPS de la liste des personnages disponibles
                    }
                }
                else
                {
                    
                    if (dps.Count == 1)
                    {
                        bool dpsTrouve = false; //On va chercher des DPS avec un rôle secondaire de DPS
                        foreach (Personnage personnage in tank)
                        {
                            if (personnage.RoleSecondaire == Personnages.Classes.Role.DPS) //On cherche un tank qui a un rôle secondaire de DPS
                            {
                                dpsTrouve = true; //On a trouvé un tank qui a un rôle secondaire de DPS
                                double nouvllemoyenne = ((moyenne + personnage.LvlSecondaire) / (equipe.Membres.Count() + 1)); //On calcule la nouvelle moyenne si on ajoute le personnage
                                double nouvelleecart = Math.Abs(nouvllemoyenne - 50);
                                if (nouvelleecart < meilleurecart) //On cherche le tank qui à la moyenne le plus proche de 50
                                {
                                    meilleurecart = nouvelleecart;
                                    personnageChoisi = personnage;
                                }
                            }
                        }
                        if (dpsTrouve)
                        {
                            equipe.AjouterMembre(personnageChoisi); //On ajoute le tank à l'équipe
                            moyenne = (moyenne + personnageChoisi.LvlSecondaire) / (equipe.Membres.Count()); //On met à jour la moyenne
                            tank.Remove(personnageChoisi); //On retire le tank de la liste des personnages disponibles
                        }
                        else
                        {
                            foreach (Personnage personnage in support)
                            {
                                if (personnage.RoleSecondaire == Personnages.Classes.Role.DPS) //On cherche un support qui a un rôle secondaire de DPS
                                {
                                    dpsTrouve = true; //On a trouvé un support qui a un rôle secondaire de DPS
                                    double nouvllemoyenne = ((moyenne + personnage.LvlSecondaire) / (equipe.Membres.Count() + 1)); //On calcule la nouvelle moyenne si on ajoute le personnage
                                    double nouvelleecart = Math.Abs(nouvllemoyenne - 50);
                                    if (nouvelleecart < meilleurecart) //On cherche le support qui à la moyenne le plus proche de 50
                                    {
                                        meilleurecart = nouvelleecart;
                                        personnageChoisi = personnage;
                                    }
                                }
                            }
                            if (dpsTrouve)
                            {
                                equipe.AjouterMembre(personnageChoisi); //On ajoute le support à l'équipe
                                moyenne = (moyenne + personnageChoisi.LvlSecondaire) / (equipe.Membres.Count()); //On met à jour la moyenne
                                support.Remove(personnageChoisi); //On retire le support de la liste des personnages disponibles
                            }
                            else
                            {
                                continuer = false; //Si on n'a pas trouvé de DPS, on arrête la répartition
                            }
                        }
                    }
                    else
                    {
                        int nbDpstrouves = 0; //On va chercher des DPS avec un rôle secondaire de DPS
                        foreach (Personnage personnage in tank)
                        {
                            if (personnage.RoleSecondaire == Personnages.Classes.Role.DPS) //On cherche un tank qui a un rôle secondaire de DPS
                            {
                                nbDpstrouves++; //On a trouvé un tank qui a un rôle secondaire de DPS
                                double nouvllemoyenne = ((moyenne + personnage.LvlSecondaire) / (equipe.Membres.Count() + 1)); //On calcule la nouvelle moyenne si on ajoute le personnage
                                double nouvelleecart = Math.Abs(nouvllemoyenne - 50);
                                if (nouvelleecart < meilleurecart) //On cherche le tank qui à la moyenne le plus proche de 50
                                {
                                    meilleurecart = nouvelleecart;
                                    personnageChoisi = personnage;
                                }
                            }
                        }
                        if (nbDpstrouves > 0)
                        {
                            equipe.AjouterMembre(personnageChoisi); //On ajoute le tank à l'équipe
                            moyenne = (moyenne + personnageChoisi.LvlSecondaire) / (equipe.Membres.Count()); //On met à jour la moyenne
                            tank.Remove(personnageChoisi); //On retire le tank de la liste des personnages disponibles
                            if (nbDpstrouves == 1)
                            {
                                //Si on a trouvé un seul DPS, on va dans les support
                                personnageChoisi = null;
                                meilleurecart = int.MaxValue;
                                foreach (Personnage personnage in support)
                                {
                                    if (personnage.RoleSecondaire == Personnages.Classes.Role.DPS) //On cherche un support qui a un rôle secondaire de DPS
                                    {
                                        double nouvllemoyenne = ((moyenne + personnage.LvlSecondaire) / (equipe.Membres.Count() + 1)); //On calcule la nouvelle moyenne si on ajoute le personnage
                                        double nouvelleecart = Math.Abs(nouvllemoyenne - 50);
                                        if (nouvelleecart < meilleurecart) //On cherche le support qui à la moyenne le plus proche de 50
                                        {
                                            meilleurecart = nouvelleecart;
                                            personnageChoisi = personnage;
                                        }
                                    }
                                }
                                if (personnageChoisi != null)
                                {
                                    equipe.AjouterMembre(personnageChoisi); //On ajoute le support à l'équipe
                                    moyenne = (moyenne + personnageChoisi.LvlSecondaire) / (equipe.Membres.Count()); //On met à jour la moyenne
                                    support.Remove(personnageChoisi); //On retire le support de la liste des personnages disponibles
                                }
                                else
                                {
                                    continuer = false; //Si on n'a pas trouvé de support, on arrête la répartition
                                }
                            }

                        }
                        else
                        {
                            foreach (Personnage personnage in support)
                            {
                                if (personnage.RoleSecondaire == Personnages.Classes.Role.DPS) //On cherche un support qui a un rôle secondaire de DPS
                                {
                                    nbDpstrouves++; //On a trouvé un support qui a un rôle secondaire de DPS
                                    double nouvllemoyenne = ((moyenne + personnage.LvlSecondaire) / (equipe.Membres.Count() + 1)); //On calcule la nouvelle moyenne si on ajoute le personnage
                                    double nouvelleecart = Math.Abs(nouvllemoyenne - 50);
                                    if (nouvelleecart < meilleurecart) //On cherche le support qui à la moyenne le plus proche de 50
                                    {
                                        meilleurecart = nouvelleecart;
                                        personnageChoisi = personnage;
                                    }
                                }
                            }
                            if (nbDpstrouves > 0)
                            {
                                equipe.AjouterMembre(personnageChoisi); //On ajoute le support à l'équipe
                                moyenne = (moyenne + personnageChoisi.LvlSecondaire) / (equipe.Membres.Count()); //On met à jour la moyenne
                                support.Remove(personnageChoisi); //On retire le support de la liste des personnages disponibles
                                if (nbDpstrouves == 1)
                                {
                                    continuer = false; //Si on a trouvé un seul DPS, on arrête la répartition
                                }
                                else
                                {
                                    //Si on a trouvé plusieurs DPS, on continue à chercher des DPS
                                    personnageChoisi = null;
                                    meilleurecart = int.MaxValue;
                                    foreach (Personnage personnage in support)
                                    {
                                        double nouvllemoyenne = ((moyenne + personnage.LvlSecondaire) / (equipe.Membres.Count() + 1)); //On calcule la nouvelle moyenne si on ajoute le personnage
                                        double nouvelleecart = Math.Abs(nouvllemoyenne - 50);
                                        if (nouvelleecart < meilleurecart) //On cherche le DPS qui à la moyenne le plus proche de 50
                                        {
                                            meilleurecart = nouvelleecart;
                                            personnageChoisi = personnage;
                                        }
                                    }
                                    if (personnageChoisi != null)
                                    {
                                        equipe.AjouterMembre(personnageChoisi); //On ajoute le DPS à l'équipe
                                        moyenne = (moyenne + personnageChoisi.LvlSecondaire) / (equipe.Membres.Count()); //On met à jour la moyenne
                                        dps.Remove(personnageChoisi); //On retire le DPS de la liste des personnages disponibles
                                    }
                                    else
                                    {
                                        continuer = false; //Si on n'a pas trouvé de DPS, on arrête la répartition
                                    }
                                }
                            }
                            else
                            {
                                continuer = false; //Si on n'a pas trouvé de DPS, on arrête la répartition
                            }
                        }
                    }
                }

                // on vérifie si l'équipe est valide
                if (equipe.EstValide(probleme))
                {
                    repartition.AjouterEquipe(equipe); //On ajoute l'équipe à la répartition



                }
                else
                {
                    continuer = false; //Si l'équipe n'est pas valide, on arrête la répartition
                }
            }

            stopwatch.Stop(); //On arrête le timer
            this.TempsExecution = stopwatch.ElapsedMilliseconds; //On enregistre le temps d'exécution de l'algorithme
            return repartition;
        }
    }
}
