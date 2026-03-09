using Application.Dominio.Contracts;
using Application.Dominio.Contracts.GoDaddy;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dominio.Builders;

/// <summary>
/// Monta o objeto de contatos para compra de domínio na GoDaddy:
/// registrant, admin e billing = dados do cliente; tech = dados fixos da empresa.
/// </summary>
[ExcludeFromCodeCoverage]
public static class GoDaddyDomainContactsBuilder
{
    /// <summary>
    /// Constrói GoDaddyDomainContacts a partir do cliente (usuário) e dos settings da empresa.
    /// Valida campos obrigatórios e lança ArgumentException com o nome do campo ausente quando aplicável.
    /// </summary>
    public static GoDaddyDomainContacts Build(CustomerDomainContactData customer, CompanyDomainSettings company)
    {
        ValidateCustomerData(customer);
        ValidateCompanySettings(company);

        var contactRegistrant = BuildCustomerContact(customer);
        var contactAdmin = BuildCustomerContact(customer);
        var contactBilling = BuildCustomerContact(customer);
        var contactTech = BuildCompanyContact(company);

        return new GoDaddyDomainContacts
        {
            ContactRegistrant = contactRegistrant,
            ContactAdmin = contactAdmin,
            ContactBilling = contactBilling,
            ContactTech = contactTech
        };
    }

    /// <summary>Monta um contato no formato GoDaddy a partir dos dados do cliente.</summary>
    public static GoDaddyContact BuildCustomerContact(CustomerDomainContactData customer)
    {
        return new GoDaddyContact
        {
            NameFirst = customer.FirstName.Trim(),
            NameLast = customer.LastName.Trim(),
            Email = customer.Email.Trim(),
            Phone = GoDaddyContactNormalizer.NormalizePhone(customer.Phone),
            AddressMailing = new GoDaddyAddressMailing
            {
                Address1 = GoDaddyContactNormalizer.NormalizeAddressField(customer.Address1),
                Address2 = GoDaddyContactNormalizer.NormalizeAddressField(customer.Address2),
                City = GoDaddyContactNormalizer.NormalizeAddressField(customer.City),
                State = GoDaddyContactNormalizer.NormalizeAddressField(customer.State),
                PostalCode = customer.PostalCode.Trim(),
                Country = GoDaddyContactNormalizer.NormalizeCountry(customer.Country)
            }
        };
    }

    /// <summary>Monta o contato técnico no formato GoDaddy a partir dos settings da empresa.</summary>
    public static GoDaddyContact BuildCompanyContact(CompanyDomainSettings company)
    {
        return new GoDaddyContact
        {
            NameFirst = company.CompanyTechFirstName.Trim(),
            NameLast = company.CompanyTechLastName.Trim(),
            Email = company.CompanyTechEmail.Trim(),
            Phone = GoDaddyContactNormalizer.NormalizePhone(company.CompanyTechPhone),
            AddressMailing = new GoDaddyAddressMailing
            {
                Address1 = GoDaddyContactNormalizer.NormalizeAddressField(company.CompanyAddress1),
                Address2 = GoDaddyContactNormalizer.NormalizeAddressField(company.CompanyAddress2),
                City = GoDaddyContactNormalizer.NormalizeAddressField(company.CompanyCity),
                State = GoDaddyContactNormalizer.NormalizeAddressField(company.CompanyState),
                PostalCode = company.CompanyPostalCode.Trim(),
                Country = GoDaddyContactNormalizer.NormalizeCountry(company.CompanyCountry)
            }
        };
    }

    private static void ValidateCustomerData(CustomerDomainContactData customer)
    {
        if (string.IsNullOrWhiteSpace(customer.FirstName)) throw new ArgumentException("FirstName do cliente é obrigatório para compra de domínio.", nameof(customer));
        if (string.IsNullOrWhiteSpace(customer.LastName)) throw new ArgumentException("LastName do cliente é obrigatório para compra de domínio.", nameof(customer));
        if (string.IsNullOrWhiteSpace(customer.Email)) throw new ArgumentException("Email do cliente é obrigatório para compra de domínio.", nameof(customer));
        if (string.IsNullOrWhiteSpace(customer.Phone)) throw new ArgumentException("Phone do cliente é obrigatório para compra de domínio.", nameof(customer));
        if (string.IsNullOrWhiteSpace(customer.Address1)) throw new ArgumentException("Address1 do cliente é obrigatório para compra de domínio.", nameof(customer));
        if (string.IsNullOrWhiteSpace(customer.City)) throw new ArgumentException("City do cliente é obrigatório para compra de domínio.", nameof(customer));
        if (string.IsNullOrWhiteSpace(customer.State)) throw new ArgumentException("State do cliente é obrigatório para compra de domínio.", nameof(customer));
        if (string.IsNullOrWhiteSpace(customer.PostalCode)) throw new ArgumentException("PostalCode do cliente é obrigatório para compra de domínio.", nameof(customer));
        if (string.IsNullOrWhiteSpace(customer.Country)) throw new ArgumentException("Country do cliente é obrigatório para compra de domínio.", nameof(customer));
    }

    private static void ValidateCompanySettings(CompanyDomainSettings company)
    {
        if (string.IsNullOrWhiteSpace(company.CompanyTechFirstName)) throw new ArgumentException("CompanyTechFirstName é obrigatório nas configurações da empresa.", nameof(company));
        if (string.IsNullOrWhiteSpace(company.CompanyTechLastName)) throw new ArgumentException("CompanyTechLastName é obrigatório nas configurações da empresa.", nameof(company));
        if (string.IsNullOrWhiteSpace(company.CompanyTechEmail)) throw new ArgumentException("CompanyTechEmail é obrigatório nas configurações da empresa.", nameof(company));
        if (string.IsNullOrWhiteSpace(company.CompanyTechPhone)) throw new ArgumentException("CompanyTechPhone é obrigatório nas configurações da empresa.", nameof(company));
        if (string.IsNullOrWhiteSpace(company.CompanyAddress1)) throw new ArgumentException("CompanyAddress1 é obrigatório nas configurações da empresa.", nameof(company));
        if (string.IsNullOrWhiteSpace(company.CompanyCity)) throw new ArgumentException("CompanyCity é obrigatório nas configurações da empresa.", nameof(company));
        if (string.IsNullOrWhiteSpace(company.CompanyState)) throw new ArgumentException("CompanyState é obrigatório nas configurações da empresa.", nameof(company));
        if (string.IsNullOrWhiteSpace(company.CompanyPostalCode)) throw new ArgumentException("CompanyPostalCode é obrigatório nas configurações da empresa.", nameof(company));
        if (string.IsNullOrWhiteSpace(company.CompanyCountry)) throw new ArgumentException("CompanyCountry é obrigatório nas configurações da empresa.", nameof(company));
    }
}
