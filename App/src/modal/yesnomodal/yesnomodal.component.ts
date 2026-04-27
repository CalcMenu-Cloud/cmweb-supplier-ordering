

import { Component, OnInit,Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
@Component({
  selector: 'app-yesnomodal',
  templateUrl: './yesnomodal.component.html',
  styleUrls: ['./yesnomodal.component.scss'],
  styles: [`
  .mat-dialog-container{
    padding: 0px!important;
    background: red !important;
    height:450px!important;
    max-height:450px!important;
  }

`]
})
export class YesnomodalComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<YesnomodalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}


  onNoClick(): void {
    this.dialogRef.close(false);
  }


  onSuccessClick(): void {
    // Perform success action
    this.dialogRef.close(true);
  }
  ngOnInit(): void {
  }


 

}
