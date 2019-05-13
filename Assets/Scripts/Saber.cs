public class SaberDamage
{
    private int slash = 45;

    private int cold;
    private int electricity;
    private int heat;
    private int toxin;

    public SaberDamage()
    {
        cold = 5;
        electricity = 5;
        heat = 5;
        toxin = 5;
    }

    public int getSlash()
    {
        return slash;
    }

    public int getCold()
    {
        return cold;
    }

    public int getElectricity()
    {
        return electricity;
    }

    public int getHeat()
    {
        return heat;
    }

    public int getToxin()
    {
        return toxin;
    }
}