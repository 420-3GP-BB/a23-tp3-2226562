using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    // Création de la classe Commande 
    public class Commande
    {
        // Elle contient un membre, le livre qu'il a commandé et le statut de sa commande (en attente ou traitée)
        public Membre LeMembre { get; set; }
        public Livre LeLivre { get; set; }
        public string Statut { get; set; }

        // constructeur de la classe commande
        public Commande(Membre membre, Livre livre, string statut) 
        {
            LeMembre = membre;
            LeLivre = livre;
            Statut = statut;
        }

        // la méthode ToString
        public override string ToString()
        {
            return $"{LeLivre} ==> {LeMembre}";
        }

    }
}
