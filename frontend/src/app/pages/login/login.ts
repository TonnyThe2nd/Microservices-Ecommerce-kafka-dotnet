import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule} from "@angular/forms";
import { Router, RouterLink } from '@angular/router';
import { User } from '../../models/User';
import { UserService } from '../../services/user-service';
@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  public form: FormGroup;
  constructor(private formBuilder: FormBuilder, private userService: UserService) {
    this.form = this.formBuilder.group({
      email: [''],
      password: ['']
    });
  }

  public login(){
    const user : User = { email: this.form.get("email")?.value, password: this.form.get("password")?.value}

    this.userService.LoginUsuario(user).subscribe({
      next: (response) => {
        console.log(response)
      },
      error: (err) => {
        console.log(err)
      }
        
    })
  }
}
