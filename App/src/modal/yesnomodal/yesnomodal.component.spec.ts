import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YesnomodalComponent } from './yesnomodal.component';

describe('YesnomodalComponent', () => {
  let component: YesnomodalComponent;
  let fixture: ComponentFixture<YesnomodalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ YesnomodalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(YesnomodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
