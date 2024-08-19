import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {NgClass, NgIf, NgOptimizedImage} from "@angular/common";
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {MatFormField} from "@angular/material/form-field";
import {MatInput} from "@angular/material/input";
import {MatLabel} from '@angular/material/form-field';
import { MatError } from '@angular/material/form-field';
import {MatButton} from "@angular/material/button";


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NgOptimizedImage, ReactiveFormsModule, MatFormField, NgIf, MatInput, MatLabel, MatError, NgClass, MatButton],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'QRPay';

  dataForm = new FormGroup({
    duznik: new FormControl('', [Validators.required]),
    racunDuznika: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[0-9\-]+$/)
    ]),
    poverilac: new FormControl('', [Validators.required]),
    racunPoverioca: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[0-9\-]+$/)
    ]),
    sifra: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[0-9]+$/)
    ]),
    valuta: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[A-Za-z]+$/)
    ]),
    iznos: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[0-9]+$/)
    ]),
    svrha: new FormControl('', [Validators.required]),
    model: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[0-9]+$/)
    ]),
    poziv: new FormControl('', [
      Validators.required,
      Validators.pattern(/^[0-9]+$/)
    ]),
    email: new FormControl('', [
      Validators.required,
      Validators.email
    ]),
  });

  sendPDF() {

  }
}
