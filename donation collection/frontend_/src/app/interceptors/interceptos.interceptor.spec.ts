import { TestBed } from '@angular/core/testing';

import { InterceptosInterceptor } from './interceptos.interceptor';

describe('InterceptosInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      InterceptosInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: InterceptosInterceptor = TestBed.inject(InterceptosInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
