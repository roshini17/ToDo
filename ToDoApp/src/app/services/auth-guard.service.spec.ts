import { HttpClientModule } from "@angular/common/http";
import { TestBed } from "@angular/core/testing";
import { RouterTestingModule } from '@angular/router/testing';
import { JwtHelperService } from "@auth0/angular-jwt";
import { AuthGuardService } from "./auth-guard.service";

const mockJwtHelperService = {
    isTokenExpired(token?: string, offsetSeconds?: number): boolean {
        return false;
    }
}

describe('AuthGuardService', () => {
    let service: AuthGuardService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientModule, RouterTestingModule],
            providers: [
                { provide: JwtHelperService, useValue: mockJwtHelperService }
            ]
        });
        service = TestBed.inject(AuthGuardService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    describe('canActivate', () => {
        it('should return true when token is present and token is valid', () => {
            localStorage.setItem('jwt', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJ0b2RvYXBpIiwibmJmIjoxNDk4MTE3NjQyLCJleHAiOjE0OTgxMjEyNDIsInVpZCI6MSwicm9sZSI6ImFkbWluIn0.ZDz_1vcIlnZz64nSM28yA1s-4c_iw3Z2ZtP-SgcYRPQ');
            const response = service.canActivate();
            expect(response).toBeTrue();
        });

        it('should load login page when token is invalid', () => {
            localStorage.setItem('jwt', '');
            spyOn(service['router'], 'navigate');
            const response = service.canActivate();
            expect(service['router'].navigate).toHaveBeenCalledWith(['login']);
            expect(response).toBeFalse();
        });
    });
});