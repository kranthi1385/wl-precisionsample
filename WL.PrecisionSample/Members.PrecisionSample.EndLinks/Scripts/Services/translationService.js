// hard coded translations
angular.module('staticTranslationsModule', ['pluggableLoader'])
  .config(function (translatePluggableLoaderProvider) {
      translatePluggableLoaderProvider
   .translations('en', {
       surveys: "SURVEYS",
       rewards: "REWARDS",
       downline: "DOWNLINE",
       myAccount: "MY ACCOUNT",
       logout: "LOGOUT",
       copyRights: "Copyright",
       terms: "Terms",
       privacy: "Privacy",
       contactUs: "Contact Us",
       aboutUs: " About Us",
       faq: "FAQ",
       siteMap: "Site Map"
   })
      .translations('es', {
          surveys: "ENCUESTAS",
          rewards: "RECOMPENSAS",
          downline: "DOWNLINE",
          myAccount: "MI CUENTA",
          logout: "CERRAR SESIÓN",
          copyRights: "derechos de autor",
          terms: "condiciones",
          privacy: "intimidad",
          contactUs: "contáctenos",
          aboutUs: "Sobre Nosotros",
          faq: "PF",
          siteMap: "Mapa del Sitio"
      })

    .translations('pt', {
        surveys: "PESQUISAS",
        rewards: "PRÊMIOS",
        downline: "DOWNLINE",
        myAccount: "MINHA CONTA",
        logout: "SAIR",
        copyRights: "direitos autorais",
        terms: "condições",
        privacy: "privacidade",
        contactUs: "Contate-Nos",
        aboutUs: "Sobre Nós",
        faq: "FAQ",
        siteMap: "Mapa do Site"
    });
  });

// a pluggable module of the application (potentially also 3rd party)
angular.module('partialModule', ['pluggableLoader'])
  .config(function (translatePluggableLoaderProvider, $translatePartialLoaderProvider) {
      // this module uses the $translatePartialLoader
      translatePluggableLoaderProvider.useLoader('$translatePartialLoader', {
          urlTemplate: '/scripts/i18n/json/{lang}/{part}.json'
      });
  });
