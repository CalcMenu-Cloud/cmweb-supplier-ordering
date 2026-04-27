import { Component, OnInit,Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
@Component({
  selector: 'app-messagemodal',
  templateUrl: './messagemodal.component.html',
  styleUrls: ['./messagemodal.component.scss']
})
export class MessagemodalComponent implements OnInit {
  @Input() title: string;
  @Input() message: string;
  @Input() customClass: string="bg-danger";
  constructor(
    public dialogRef: MatDialogRef<MessagemodalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit(): void {
  }


  close(): void {
    this.dialogRef.close(false);
  }

}
