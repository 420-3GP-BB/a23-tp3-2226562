﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Commande
    {
        public Membre LeMembre { get; set; }
        public Livre LeLivre { get; set; }
        public string Statut { get; set; }

        public Commande(Membre membre, Livre livre, string statut) 
        {
            LeMembre = membre;
            LeLivre = livre;
            Statut = statut;
        }

        public override string ToString()
        {
            return $"{LeLivre} ==> {LeMembre}";
        }

    }
}