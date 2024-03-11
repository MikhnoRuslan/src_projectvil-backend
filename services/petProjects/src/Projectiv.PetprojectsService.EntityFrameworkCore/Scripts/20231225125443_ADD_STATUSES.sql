DO $$
DECLARE
    NameTranslationId1 UUID DEFAULT uuid_generate_v4();
    NameTranslationId2 UUID DEFAULT uuid_generate_v4();
BEGIN
    INSERT INTO public."PetProject.Statuses"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId1);
    INSERT INTO public."PetProject.Statuses"("Id", "NameTranslationId") VALUES (uuid_generate_v4(), NameTranslationId2);  
    
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId1, 1, 'В работе');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId1, 2, 'In progress');

    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId2, 1, 'Завершен');
    INSERT INTO public."PetProject.Translations"("Id", "Language", "Translate") VALUES (NameTranslationId2, 2, 'Completed');
END $$;