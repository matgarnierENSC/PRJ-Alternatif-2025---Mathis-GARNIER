public class ZoneMurale : Zone
{
    public ZoneMurale(int largeur, int hauteur)
{
    Largeur = largeur;
    Hauteur = hauteur;
}


    public override bool EstCompatible(Oeuvre oeuvre)
    {
        if (oeuvre is not Toile toile) return false;
        return EstLibre 
    && Largeur >= toile.Largeur + 40 
    && Hauteur >= toile.Hauteur + 40;

    }
}