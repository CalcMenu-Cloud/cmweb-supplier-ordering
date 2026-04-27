import { enableProdMode, TRANSLATIONS, TRANSLATIONS_FORMAT } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
    // Override console.log in production mode
    // window.console.log = () => {};
  enableProdMode();
}

//platformBrowserDynamic().bootstrapModule(AppModule)
  //.catch(err => console.error(err));

  platformBrowserDynamic().bootstrapModule(AppModule, {
    providers: [
      { provide: TRANSLATIONS, useValue: environment.api_url },
      { provide: TRANSLATIONS_FORMAT, useValue: 'xlf' }
    ]
  });
