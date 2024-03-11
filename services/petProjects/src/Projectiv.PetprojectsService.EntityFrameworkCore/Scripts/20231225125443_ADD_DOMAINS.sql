CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

DO $$
DECLARE
    NameTranslationId1 UUID DEFAULT uuid_generate_v4();
    NameTranslationId2 UUID DEFAULT uuid_generate_v4();
    NameTranslationId3 UUID DEFAULT uuid_generate_v4();
    NameTranslationId4 UUID DEFAULT uuid_generate_v4();
    NameTranslationId5 UUID DEFAULT uuid_generate_v4();
    NameTranslationId6 UUID DEFAULT uuid_generate_v4();
    NameTranslationId7 UUID DEFAULT uuid_generate_v4();
    NameTranslationId8 UUID DEFAULT uuid_generate_v4();
    NameTranslationId9 UUID DEFAULT uuid_generate_v4();
    NameTranslationId10 UUID DEFAULT uuid_generate_v4();
    NameTranslationId11 UUID DEFAULT uuid_generate_v4();
    NameTranslationId12 UUID DEFAULT uuid_generate_v4();
    NameTranslationId13 UUID DEFAULT uuid_generate_v4();
    NameTranslationId14 UUID DEFAULT uuid_generate_v4();
    NameTranslationId15 UUID DEFAULT uuid_generate_v4();
BEGIN
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId1);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId2);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId3);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId4);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId5);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId6);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId7);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId8);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId9);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId10);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId11);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId12);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId13);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId14);
    INSERT INTO public."PetProject.Domains"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId15);

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId1, 1, 'E-commerce');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId1, 2, 'E-commerce');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId2, 1, 'HealthTech');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId2, 2, 'HealthTech');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId3, 1, 'Legaltech');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId3, 2, 'Legaltech');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId4, 1, 'Transport');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId4, 2, 'Transport');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId5, 1, 'Fashion&Beauty');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId5, 2, 'Fashion&Beauty');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId6, 1, 'Pet и Эко');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId6, 2, 'Pet и Эко');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId7, 1, 'Retail');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId7, 2, 'Retail');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId8, 1, 'GameTech');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId8, 2, 'GameTech');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId9, 1, 'FinTech');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId9, 2, 'FinTech');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId10, 1, 'CyberSecurity');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId10, 2, 'CyberSecurity');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId11, 1, 'EdTech');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId11, 2, 'EdTech');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId12, 1, 'AgroTech');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId12, 2, 'AgroTech');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId13, 1, 'FoodTech');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId13, 2, 'FoodTech');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId14, 1, 'Travel');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId14, 2, 'Travel');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId15, 1, 'Другое');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId15, 2, 'Other');
END $$;
