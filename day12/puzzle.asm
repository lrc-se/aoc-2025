section .text

global CountSpaciousAreas
export CountSpaciousAreas

int_size equ 4

CountSpaciousAreas:
        xor     eax, eax
        xor     r10d, r10d
.loop:
        mov     r8d, [rdx]
        mul     r8, 0x55555556
        shr     r8, 32
        add     rdx, int_size
        mov     r9d, [rdx]
        mul     r9, 0x55555556
        shr     r9, 32
        mul     r8d, r9d

        add     rdx, int_size
        mov     r9d, [rdx]
        add     r9d, [rdx+int_size]
        add     r9d, [rdx+int_size*2]
        add     r9d, [rdx+int_size*3]
        add     r9d, [rdx+int_size*4]
        add     r9d, [rdx+int_size*5]

        cmp     r8d, r9d
        setae   r10b
        add     eax, r10d

        add     rdx, int_size*6
        dec     ecx
        jnz     .loop
        ret
