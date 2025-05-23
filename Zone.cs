// Classe abstraite représentant une zone d'exposition dans un mur (murale ou vitrine)
public abstract class Zone
{
    public int Largeur { get; set; }  // Largeur de la zone
    public int Hauteur { get; set; }  // Hauteur de la zone
    public bool EstLibre { get; set; } = true; // Indique si la zone est disponible

  
    public abstract bool EstCompatible(Oeuvre oeuvre);

    // Marque la zone comme occupée
    public void Occuper()
    {
        EstLibre = false;
    }
}
