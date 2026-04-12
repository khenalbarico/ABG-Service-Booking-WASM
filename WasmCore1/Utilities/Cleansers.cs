namespace WasmCore1.Utilities;

public static class Cleansers
{
    public static string NormalizeCategory(this string category)
    {
        return category?.ToLower() switch
        {
            "nails"    => "Nails",
            "lash"     => "Lash",
            "eyebrows" => "Eyebrows",
            "footspa"  => "Footspa",
            _          => "Nails"
        };
    }
}
