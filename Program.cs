﻿ Console.OutputEncoding = System.Text.Encoding.UTF8;

        var jeu = new Jeu();
        jeu.InitialiserMusee(); // ✅ Initialisation UNE seule fois
        jeu.Lancer();           // ✅ Ensuite on lance le menu

