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
    public class AlgorithmeMyenne3_role_et_sousrole : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Repartition repartition = new Repartition(jeuTest);
            // On crée une liste de personnages à partir des personnages du jeu de test
            List<Personnage> personnages = new List<Personnage>(jeuTest.Personnages);

            Boolean continuer = true;
            while (continuer)
            {
                Equipe equipe = new Equipe();// On crée une nouvelle équipe
                Personnage personnagechoisi = null;// On initialise le personnage choisi à null
                double meilleurecart = 100;// On initialise la meilleure écart à 100
                Boolean supporttrouve = false;// On initialise le support trouvé à false
                double moyenne = 0;// On initialise la moyenne à 0
                foreach (Personnage personnage in personnages)// on parcourt tous les personnages
                {
                    if (personnage.RolePrincipal == Personnages.Classes.Role.SUPPORT || personnage.RoleSecondaire == Personnages.Classes.Role.SUPPORT)
                    {
                        supporttrouve = true;
                        double ecart;
                        if (personnage.RolePrincipal == Personnages.Classes.Role.SUPPORT)
                        {
                            ecart = Math.Abs(personnage.LvlPrincipal - 50);
                            if (ecart < meilleurecart)
                            {
                                meilleurecart = ecart;
                                personnagechoisi = personnage;
                            }
                        }
                        else
                        {
                            ecart = Math.Abs(personnage.LvlSecondaire - 50);
                            if (ecart < meilleurecart)
                            {
                                meilleurecart = ecart;
                                personnagechoisi = personnage;
                            }
                        }
                    }
                }
                if (supporttrouve)
                {
                    personnages.Remove(personnagechoisi);// On retire le personnage choisi de la liste des personnages
                    equipe.AjouterMembre(personnagechoisi);// On ajoute le personnage choisi à l'équipe
                    if (personnagechoisi.RolePrincipal == Personnages.Classes.Role.SUPPORT)
                    {
                        moyenne = personnagechoisi.LvlPrincipal;
                    }
                    else
                    {
                        moyenne = personnagechoisi.LvlSecondaire;
                    }
                }
                else
                {
                    continuer = false;// Si aucun support n'est trouvé, on arrête la boucle
                }

                if (continuer) //pas la peine de contineur si il n'y a pas de support
                {
                    //on cherche maintenant le tank
                    personnagechoisi = null;// On réinitialise le personnage choisi à null
                    meilleurecart = 100;// On réinitialise la meilleure écart à 100
                    Boolean tanktrouve = false;
                    foreach (Personnage personnage in personnages)
                    {
                        if (personnage.RolePrincipal == Personnages.Classes.Role.TANK || personnage.RoleSecondaire == Personnages.Classes.Role.TANK)
                        {
                            tanktrouve = true;
                            double ecart;
                            double nouvellemoyenne = 0;
                            if (personnage.RolePrincipal == Personnages.Classes.Role.TANK)
                            {
                                nouvellemoyenne = (moyenne + personnage.LvlPrincipal) / 2;
                            }
                            else
                            {
                                moyenne = (moyenne + personnage.LvlSecondaire) / 2;
                            }
                            ecart = Math.Abs(nouvellemoyenne - 50);
                            if (ecart < meilleurecart)
                            {
                                meilleurecart = ecart;
                                personnagechoisi = personnage;
                            }
                        }
                    }
                    if (tanktrouve)
                    {
                        personnages.Remove(personnagechoisi);// On retire le personnage choisi de la liste des personnages
                        equipe.AjouterMembre(personnagechoisi);// On ajoute le personnage choisi à l'équipe
                        if (personnagechoisi.RolePrincipal == Personnages.Classes.Role.TANK)
                        {
                            moyenne = (moyenne + personnagechoisi.LvlPrincipal) / 2;
                        }
                        else
                        {
                            moyenne = (moyenne + personnagechoisi.LvlSecondaire) / 2;
                        }
                    }
                    else
                    {
                        continuer = false;// Si aucun tank n'est trouvé, on arrête la boucle
                    }
                }

                if (continuer)
                {
                    //on cherche maintenant les DPS
                    for (int i = 0; i < 2; i++)
                    {
                        personnagechoisi = null;// On réinitialise le personnage choisi à null
                        meilleurecart = 100;// On réinitialise la meilleure écart à 100
                        Boolean dpstrouve = false;
                        foreach (Personnage personnage in personnages)
                        {
                            if (personnage.RolePrincipal == Personnages.Classes.Role.DPS || personnage.RoleSecondaire == Personnages.Classes.Role.DPS)
                            {
                                dpstrouve = true;
                                double ecart;
                                double nouvellemoyenne = 0;
                                if (personnage.RolePrincipal == Personnages.Classes.Role.DPS)
                                {
                                    nouvellemoyenne = (moyenne + personnage.LvlPrincipal) / (equipe.Membres.Count() + 1);
                                }
                                else
                                {
                                    nouvellemoyenne = (moyenne + personnage.LvlSecondaire) / (equipe.Membres.Count() + 1);
                                }
                                ecart = Math.Abs(nouvellemoyenne - 50);
                                if (ecart < meilleurecart)
                                {
                                    meilleurecart = ecart;
                                    personnagechoisi = personnage;
                                }
                            }
                        }
                        if (dpstrouve)
                        {
                            personnages.Remove(personnagechoisi);// On retire le personnage choisi de la liste des personnages
                            equipe.AjouterMembre(personnagechoisi);// On ajoute le personnage choisi à l'équipe
                            if (personnagechoisi.RolePrincipal == Personnages.Classes.Role.DPS)
                            {
                                moyenne = (moyenne + personnagechoisi.LvlPrincipal) / equipe.Membres.Count();
                            }
                            else
                            {
                                moyenne = (moyenne + personnagechoisi.LvlSecondaire) / equipe.Membres.Count();
                            }
                        }
                        else
                        {
                            continuer = false;// Si aucun DPS n'est trouvé, on arrête la boucle
                        }
                    }
                }

                Probleme probleme = new Probleme();
                probleme = Probleme.ROLESECONDAIRE;
                if (equipe.EstValide(probleme))
                {
                    repartition.AjouterEquipe(equipe);// On ajoute l'équipe à la répartition si elle est valide
                }
                else
                {
                    // Si l'équipe n'est pas valide, on arrête la boucle
                    continuer = false;
                }
            }
            stopwatch.Stop();
            TempsExecution = stopwatch.ElapsedMilliseconds;
            return repartition;
        }
    }
}
