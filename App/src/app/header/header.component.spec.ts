import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { HeaderComponent } from './header.component';
import { GlobalvarService } from '../services/globalvar.service';
import { HogashopapiService } from '../services/hogashopapi.service';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;
  const mockGlobalvarService = jasmine.createSpyObj('GlobalvarService', [
    'checkSession',
    'getuserinfojson',
    'clearSession'
  ]);

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [ HeaderComponent ],
      providers: [
        { provide: GlobalvarService, useValue: mockGlobalvarService },
        HogashopapiService
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
