﻿using System;

namespace dotNES
{
    partial class CPU
    {
        public void Initialize()
        {
            A = 0;
            X = 0;
            Y = 0;
            SP = 0xFD;
            P = 0x24;

            PC = 0xC000;
        }

        public void Reset()
        {
            SP -= 3;
            F.BreakSource = true;
        }

        public void ExecuteSingleInstruction()
        {
            currentInstruction = NextByte();

            currentAddressor = instructionAddressingModes[currentInstruction];
            currentAddressor?.Reset();

            // if (cycle >= 4900)
            Console.WriteLine($"{(PC - 1).ToString("X4")}  {currentInstruction.ToString("X2")}	\t\t\tA:{A.ToString("X2")} X:{X.ToString("X2")} Y:{Y.ToString("X2")} P:{P.ToString("X2")} SP:{SP.ToString("X2")}");

            switch (currentInstruction)
            {
                case 0x4C: // JMP
                    PC = NextWord();
                    break;
                case 0xA9: // LDA
                    LDA();
                    break;
                case 0xA5: // LDA
                    LDA();
                    break;
                case 0xAD: // LDA
                    LDA();
                    break;
                case 0xA0: // LDY
                    LDY();
                    break;
                case 0xA4: // LDY
                    LDY();
                    break;
                case 0xA2: // LDX
                    LDX();
                    break;
                case 0xA6: // LDX
                    LDX();
                    break;
                case 0xAE: // LDX
                    LDX();
                    break;
                case 0xAC: // LDY
                    LDY();
                    break;
                case 0x86: // STX
                    WriteByte(NextByte(), X);
                    break;
                case 0x8E: // STX
                    WriteByte(NextWord(), X);
                    break;
                case 0x84: // STY
                    WriteByte(NextByte(), Y);
                    break;
                case 0x8C: // STY
                    WriteByte(NextWord(), Y);
                    break;
                case 0x85: // STA
                    WriteByte(NextByte(), A);
                    break;
                case 0x8D: // STA
                    WriteByte(NextWord(), A);
                    break;
                case 0x20: // JSR
                    PushWord(PC + 1);
                    PC = NextWord();
                    break;
                case 0x60: // RTS
                    PC = PopWord() + 1;
                    break;
                case 0xEA: // NOP
                    NOP();
                    break;
                case 0x18: // CLC
                    CLC();
                    break;
                case 0x38: // SEC
                    SEC();
                    break;
                case 0x58: // CLI
                    CLI();
                    break;
                case 0x78: // SEI
                    SEI();
                    break;
                case 0xB8: // CLV
                    CLV();
                    break;
                case 0xD8: // CLD
                    CLD();
                    break;
                case 0xF8: // SED
                    SED();
                    break;
                case 0xB0: // BCS
                    BCS();
                    break;
                case 0x90: // BCC
                    BCC();
                    break;
                case 0xF0: // BEQ
                    BEQ();
                    break;
                case 0xD0: // BNE
                    BNE();
                    break;
                case 0x70: // BVS
                    BVS();
                    break;
                case 0x50: // BVC
                    BVC();
                    break;
                case 0x10: // BPL
                    BPL();
                    break;
                case 0x30: // BMI
                    BMI();
                    break;
                case 0x24: // BIT
                    BIT();
                    break;
                case 0x2C: // BIT
                    BIT();
                    break;
                case 0x08: // PHP
                    Push(P | BreakSourceBit);
                    break;
                case 0x48: // PHA
                    Push(A);
                    break;
                case 0x28: // PLP
                    P = Pop() & ~BreakSourceBit;
                    break;
                case 0x68: // PLA
                    A = Pop();
                    break;
                case 0x29: // AND
                    AND();
                    break;
                case 0x09: // OR
                    ORA();
                    break;
                case 0x49: // EOR
                    EOR();
                    break;
                case 0x69: // ADC
                    ADC();
                    break;
                case 0xE9: // SBC
                    SBC();
                    break;
                case 0xC9: // CMP
                    CMP(A);
                    break;
                case 0xC0: // CPY
                    CMP(Y);
                    break;
                case 0xE0: // CPX
                    CMP(X);
                    break;
                case 0xC8: // INY
                    Y++;
                    break;
                case 0x88: // DEY
                    Y--;
                    break;
                case 0xE8: // INX
                    X++;
                    break;
                case 0xCA: // DEX
                    X--;
                    break;
                case 0xA8: // TAY
                    Y = A;
                    break;
                case 0x98: // TYA
                    A = Y;
                    break;
                case 0xAA: // TAX
                    X = A;
                    break;
                case 0x8A: // TXA
                    A = X;
                    break;
                case 0xBA: // TSX
                    X = SP;
                    break;
                case 0x9A: // TXS
                    SP = X;
                    break;
                case 0x40: // RTI
                    P = Pop();
                    PC = PopWord();
                    break;
                case 0x4A: // LSR
                    F.Carry = (A & 0x1) > 0;
                    A >>= 1;
                    break;
                case 0x0A: // ASL
                    F.Carry = (A & 0x80) > 0;
                    A <<= 1;
                    break;
                case 0x6A: // ROR
                    bool c = F.Carry;
                    F.Carry = (A & 0x1) > 0;
                    A >>= 1;
                    if (c) A |= 0x80;
                    break;
                case 0x2A: // ROL
                    c = F.Carry;
                    F.Carry = (A & 0x80) > 0;
                    A <<= 1;
                    if (c) A |= 0x1;
                    break;
                case 0xA1: // LDA ind
                    LDA();
                    break;
                case 0x81: // STA ind
                    WriteByte(indX, A);
                    break;
                case 0x01: // ORA ind
                    ORA();
                    break;
                case 0x21: // AND ind
                    AND();
                    break;
                case 0x41: // EOR ind
                    EOR();
                    break;
                case 0x61: // ADC ind
                    ADC();
                    break;
                case 0xE1: // SBC ind
                    SBC();
                    break;
                case 0xC1: // CMP ind
                    CMP(A);
                    break;
                case 0x05: // ORA
                    ORA();
                    break;
                case 0x25: // AND
                    AND();
                    break;
                case 0x45: // EOR
                    EOR();
                    break;
                case 0x65: // ADC
                    ADC();
                    break;
                case 0xE5: // SBC
                    SBC();
                    break;
                case 0xC5: // CMP
                    CMP(A);
                    break;
                case 0xE4: // CPX
                    CMP(X);
                    break;
                case 0xC4: // CPY
                    CMP(Y);
                    break;
                case 0x46: // LSR
                    LSR();
                    break;
                case 0x06: // ASL
                    ASL();
                    break;
                case 0x66: // ROR
                    ROR();
                    break;
                case 0x26: // ROL
                    ROL();
                    break;
                case 0xE6: // INC
                    INC();
                    break;
                case 0xC6: // DEC
                    DEC();
                    break;
                case 0x0D: // ORA
                    ORA();
                    break;
                case 0x2D: // AND
                    AND();
                    break;
                case 0x4D: // EOR
                    EOR();
                    break;
                case 0x6D: // ADC
                    ADC();
                    break;
                case 0xED: // SBC
                    SBC();
                    break;
                case 0xCD: // CMP
                    CMP(A);
                    break;
                case 0xEC: // CPX
                    CMP(X);
                    break;
                case 0xCC: // CPY
                    CMP(Y);
                    break;
                case 0x4E: // LSR
                    LSR();
                    break;
                case 0x0E: // ASL
                    ASL();
                    break;
                case 0x6E: // ROR
                    ROR();
                    break;
                case 0x2E: // ROL
                    ROL();
                    break;
                case 0xEE: // INC
                    INC();
                    break;
                case 0xCE: // DEC
                    DEC();
                    break;
                case 0xB1: // LDA
                    LDA();
                    break;
                case 0x11: // ORA
                    ORA();
                    break;
                case 0x31: // AND
                    AND();
                    break;
                case 0x51: // EOR
                    EOR();
                    break;
                case 0xF1: // SBC
                    SBC();
                    break;
                case 0x71: // ADC
                    ADC();
                    break;
                case 0x91: // STA
                    WriteByte(indY, A);
                    break;
                case 0xD1: // CMP
                    CMP(A);
                    break;
                case 0x6C: // JMP
                    int off = NextWord();
                    // AN INDIRECT JUMP MUST NEVER USE A VECTOR BEGINNING ON THE LAST BYTE OF A PAGE
                    //
                    // If address $3000 contains $40, $30FF contains $80, and $3100 contains $50, 
                    // the result of JMP ($30FF) will be a transfer of control to $4080 rather than
                    // $5080 as you intended i.e. the 6502 took the low byte of the address from
                    // $30FF and the high byte from $3000.
                    //
                    // http://www.6502.org/tutorials/6502opcodes.html
                    int hi = (off & 0xFF) == 0xFF ? off - 0xFF : off + 1;
                    PC = ReadByte(off) | (ReadByte(hi) << 8);
                    break;
                case 0xB9: // LDA
                    LDA();
                    break;
                case 0x19: // ORA
                    ORA();
                    break;
                case 0x39: // AND
                    AND();
                    break;
                case 0x59: // EOR
                    EOR();
                    break;
                case 0x79: // ADC
                    ADC();
                    break;
                case 0xF9: // SBC
                    SBC();
                    break;
                case 0xD9: // CMP
                    CMP(A);
                    break;
                case 0x99: // STA
                    WriteByte(absY, A);
                    break;
                case 0xB4: // LDY
                    LDY();
                    break;
                case 0x94: // STY
                    WriteByte(zpgX, Y);
                    break;
                case 0x15: // ORA
                    ORA();
                    break;
                case 0x35: // AND
                    AND();
                    break;
                case 0x55: // EOR
                    EOR();
                    break;
                case 0x75: // ADC
                    ADC();
                    break;
                case 0xF5: // SBC
                    SBC();
                    break;
                case 0xD5: // CMP
                    CMP(A);
                    break;
                case 0xB5: // LDA
                    LDA();
                    break;
                case 0x95: // STA
                    WriteByte(zpgX, A);
                    break;
                case 0x56: // LSR
                    LSR();
                    break;
                case 0x16: // ASL
                    ASL();
                    break;
                case 0x76: // ROR
                    ROR();
                    break;
                case 0x36: // ROL
                    ROL();
                    break;
                case 0xF6: // INC
                    INC();
                    break;
                case 0xD6: // DEC
                    DEC();
                    break;
                case 0xB6: // LDX
                    LDX();
                    break;
                case 0x96: // STX
                    WriteByte(zpgY, X);
                    break;
                case 0xBC: // LDY
                    LDY();
                    break;
                case 0xBE: // LDX
                    LDX();
                    break;
                case 0x1D: // ORA
                    ORA();
                    break;
                case 0x3D: // AND
                    AND();
                    break;
                case 0x5D: // EOR
                    EOR();
                    break;
                case 0x7D: // ADC
                    ADC();
                    break;
                case 0xFD: // SBC
                    SBC();
                    break;
                case 0xDD: // CMP
                    CMP(A);
                    break;
                case 0xBD: // LDA
                    LDA();
                    break;
                case 0x9D: // STA
                    WriteByte(absX, A);
                    break;
                case 0x5E: // LSR
                    LSR();
                    break;
                case 0x1E: // ASL
                    ASL();
                    break;
                case 0x7E: // ROR
                    ROR();
                    break;
                case 0x3E: // ROL
                    ROL();
                    break;
                case 0xFE: // INC
                    INC();
                    break;
                case 0xDE: // DEC
                    DEC();
                    break;
                /*case 0x00: // BRK
                    NextByte();
                    PushWord(PC);
                    flags.IRQ = true;
                    Push(P);
                    PC = ReadAddress(0xFFFE) | (ReadAddress(0xFFFF) << 8);
                    break;*/
                default:
                    throw new ArgumentException(currentInstruction.ToString("X2"));
            }
        }

        private void WriteByte(Addressor accessor, int val)
        {
            accessor.Write(val);
        }
    }
}
