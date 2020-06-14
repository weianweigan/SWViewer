import { TestBed } from '@angular/core/testing';

import { SwfileService } from './swfile.service';

describe('SwfileService', () => {
  let service: SwfileService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SwfileService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
