using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Dominio.Builders;

/// <summary>
/// Normaliza dados para o formato exigido pela API GoDaddy (domains/purchase).
/// Telefone: +XX.NNNNNNNNN (pattern /^\+([0-9]){1,3}\.([0-9] ?){5,14}$/).
/// País: código ISO 2 letras (ex.: BR).
/// address2: string (API não aceita null).
/// City/State/Address: apenas letras, números, espaços e -.,' (remove acentos).
/// </summary>
[ExcludeFromCodeCoverage]
public static class GoDaddyContactNormalizer
{
    private static readonly Regex DigitsOnly = new(@"[^\d]", RegexOptions.Compiled);
    /// <summary>GoDaddy: city/address só aceitam letters, numbers, spaces e -.,'</summary>
    private static readonly Regex AllowedAddressChars = new(@"[^\w\s\-.,']", RegexOptions.Compiled);

    /// <summary>
    /// Normaliza campo de endereço para o padrão GoDaddy: apenas letras, números, espaços e -.,'
    /// Remove acentos (São Paulo → Sao Paulo) e qualquer outro caractere não permitido.
    /// </summary>
    public static string NormalizeAddressField(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;
        var trimmed = value.Trim();
        var withoutDiacritics = RemoveDiacritics(trimmed);
        return AllowedAddressChars.Replace(withoutDiacritics, "");
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);
        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Normaliza telefone para o formato GoDaddy: +[1-3 dígitos].[5-14 dígitos].
    /// Ex.: "(11) 99999-9999" ou "11999999999" → "+55.11999999999" (Brasil).
    /// </summary>
    /// <param name="phone">Telefone como armazenado (pode ter parênteses, traços, espaços).</param>
    /// <param name="defaultCountryCode">Código do país quando o número não o inclui (Brasil = 55).</param>
    /// <returns>Telefone no formato +XX.NNNNNNNNN.</returns>
    public static string NormalizePhone(string? phone, string defaultCountryCode = "55")
    {
        if (string.IsNullOrWhiteSpace(phone))
            return $"+{defaultCountryCode}.0000000000";

        var digits = DigitsOnly.Replace(phone, "");
        if (string.IsNullOrEmpty(digits))
            return $"+{defaultCountryCode}.0000000000";

        // Brasil: 10 ou 11 dígitos = DDD + número → prefixar 55
        if (digits.Length >= 10 && digits.Length <= 11 && defaultCountryCode == "55")
            digits = "55" + digits;
        // Já tem código do país (ex.: 5511999999999)
        else if (digits.Length < 10)
            digits = defaultCountryCode + digits;

        // Garantir 1–3 dígitos de país + ponto + 5–14 dígitos
        var countryCode = defaultCountryCode;
        if (digits.StartsWith("55") && digits.Length >= 12)
        {
            countryCode = "55";
            digits = digits.Substring(2);
        }
        else if (digits.Length > 3)
        {
            var len = Math.Min(3, digits.Length - 5);
            if (len >= 1)
            {
                countryCode = digits.Substring(0, len);
                digits = digits.Substring(len);
            }
        }

        if (digits.Length > 14)
            digits = digits.Substring(0, 14);
        if (digits.Length < 5)
            digits = digits.PadRight(5, '0');

        return $"+{countryCode}.{digits}";
    }

    /// <summary>
    /// Normaliza país para código ISO 2 letras (ex.: Brasil → BR). GoDaddy exige valor da lista de códigos.
    /// </summary>
    public static string NormalizeCountry(string? country)
    {
        if (string.IsNullOrWhiteSpace(country))
            return "BR";

        var trimmed = country.Trim();
        if (trimmed.Length == 2)
            return trimmed.ToUpperInvariant();

        return TryMapCountryNameToIso2(trimmed) ?? "BR";
    }

    /// <summary>
    /// Garante que address2 seja sempre string (API GoDaddy não aceita null em address2).
    /// </summary>
    public static string NormalizeAddress2(string? address2) =>
        string.IsNullOrWhiteSpace(address2) ? string.Empty : address2.Trim();

    private static string? TryMapCountryNameToIso2(string name)
    {
        var n = name.ToUpperInvariant();
        return n switch
        {
            "BRASIL" or "BRAZIL" => "BR",
            "ESTADOS UNIDOS" or "EUA" or "UNITED STATES" => "US",
            "PORTUGAL" => "PT",
            "ARGENTINA" => "AR",
            "PARAGUAI" or "PARAGUAY" => "PY",
            "URUGUAI" or "URUGUAY" => "UY",
            "CHILE" => "CL",
            "COLOMBIA" => "CO",
            "PERU" => "PE",
            "MEXICO" or "MÉXICO" => "MX",
            "ESPANHA" or "SPAIN" => "ES",
            "FRANCA" or "FRANCE" => "FR",
            "ALEMANHA" or "GERMANY" => "DE",
            "ITALIA" or "ITALY" => "IT",
            "REINO UNIDO" or "UK" or "UNITED KINGDOM" => "GB",
            _ => null
        };
    }
}
