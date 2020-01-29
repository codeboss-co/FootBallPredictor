import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamsSearchContainer } from './teams-search-container.component';

describe('TeamsSearchContainerComponent', () => {
  let component: TeamsSearchContainer;
  let fixture: ComponentFixture<TeamsSearchContainer>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TeamsSearchContainer ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TeamsSearchContainer);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
