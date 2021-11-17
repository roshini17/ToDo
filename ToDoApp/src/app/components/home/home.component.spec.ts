import { ComponentFixture, TestBed } from "@angular/core/testing";
import { JwtHelperService } from "@auth0/angular-jwt";
import { HomeComponent } from "./home.component";

const mockJwtHelperService = {
    isTokenExpired(token?: string, offsetSeconds?: number): boolean {
        return false;
    }
}

describe('HomeComponent', () => {
    let component: HomeComponent;
    let fixture: ComponentFixture<HomeComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [HomeComponent],
            providers: [
                { provide: JwtHelperService, useValue: mockJwtHelperService }
            ]
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(HomeComponent);
        component = fixture.componentInstance;

        fixture.detectChanges();
    });

    afterEach(() => {
        component.logOut();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('isUserAuthenticated', () => {
        it('should return true when token is present and token is valid', () => {
            localStorage.setItem('jwt', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJ0b2RvYXBpIiwibmJmIjoxNDk4MTE3NjQyLCJleHAiOjE0OTgxMjEyNDIsInVpZCI6MSwicm9sZSI6ImFkbWluIn0.ZDz_1vcIlnZz64nSM28yA1s-4c_iw3Z2ZtP-SgcYRPQ');
            const isUserAuthenticated = component.isUserAuthenticated();
            expect(isUserAuthenticated).toBeTrue();
        });

        it('should return false when token is invalid', () => {
            localStorage.setItem('jwt', '');
            const isUserAuthenticated = component.isUserAuthenticated();
            expect(isUserAuthenticated).toBeFalse();
        });
    });

    describe('logOut', () => {
        it('should remove the token from localstorage', () => {
            localStorage.setItem('jwt', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJ0b2RvYXBpIiwibmJmIjoxNDk4MTE3NjQyLCJleHAiOjE0OTgxMjEyNDIsInVpZCI6MSwicm9sZSI6ImFkbWluIn0.ZDz_1vcIlnZz64nSM28yA1s-4c_iw3Z2ZtP-SgcYRPQ');
            component.logOut();
            expect(localStorage.getItem('jwt')).toBeNull();
        });
    });
});