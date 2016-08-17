# Default Value Property Editor

Umbraco 7.4.3+

- Creates a Data Type with a Default Value (used for properties that cannot be modified)

This was useful for a project that implements the schema.org types, so each page (article, news, foto) has a default value (Article, newsArticle, Photograph) that may be overridden by another schema.org object type, but if the user does not override anything, the default is used. Therefore, the default cannot be changed.

There can be plenty of other reasons to add a "cannot be modified" property to a document, so the package is now public.

Feedback is appreciated
[Carlos Casalicchio](mailto:carlos.casalicchio@gmail.com)