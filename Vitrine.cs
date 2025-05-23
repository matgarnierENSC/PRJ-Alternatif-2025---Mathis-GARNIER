// Représente une zone de type vitrine destinée à exposer une antiquité
public class Vitrine : Zone
{
    public int CapaciteMax { get; } // Capacité maximale de la vitrine en kg

    public Vitrine(int capaciteMax)
    {
        CapaciteMax = capaciteMax;
    }

    // Vérifie si l'œuvre peut être placée dans cette vitrine
    public override bool EstCompatible(Oeuvre oeuvre)
    {
        // Seules les antiquités peuvent aller dans une vitrine
        if (oeuvre is not Antiquite antiquite) return false;

        // La vitrine doit être libre et supporter le poids de l'antiquité
        return EstLibre && CapaciteMax >= antiquite.Poids;
    }
}
